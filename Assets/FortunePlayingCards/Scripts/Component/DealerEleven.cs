using System.Collections;
using System.Collections.Generic;
using FortunePlayingCards.Scripts.Component;
using System.Linq;
using FortunePlayingCards.Scripts.UI;
using UnityEngine;

public class DealerEleven : MonoBehaviour
{
    public DealerElevenUI DealerUIInstance { get; set; }

    [SerializeField]
    private CardDeckEleven _cardDeck;

    [SerializeField]
    private CardSlotEleven _rightHandCardSlot;

    [SerializeField]
    private CardSlotEleven _leftHandCardSlot;

    [SerializeField]
    private CardSlotEleven _stackCardSlot;

    [SerializeField]
    public CardSlotEleven _currentCardSlot;

    [SerializeField]
    private CardSlotEleven _discardCardSlot;

    [SerializeField]
    private List<CardSlotEleven> _middleCardSlots;
    
    public int playerScore = 0;
    public bool isDrawFinished = false;
    
    public static List<string> cardNames = new List<string>();

    private const float CardStackDelay = .01f;

    public CardInfo CardInfo;
    
    public int DealInProgress { get; set; }

    private void Awake()
    {
        _cardDeck.InstantiateDeck();
        StartCoroutine(StackCardRangeOnSlot(0, _cardDeck.CardList.Count, _stackCardSlot));
    }
    
    private void MoveCardSlotToCardSlot(CardSlotEleven sourceCardSlot, CardSlotEleven targetCardSlot)
    {
        Card cardNewNew;
        while ((cardNewNew = sourceCardSlot.TopCard()) != null)
        {
            targetCardSlot.AddCard(cardNewNew);
        }
    }
    
    private IEnumerator StackCardRangeOnSlot(int start, int end, CardSlotEleven cardSlot)
    {
        DealInProgress++;
        for (int i = start; i < end; ++i)
        {
            cardSlot.AddCard(_cardDeck.CardList[i]);
            yield return new WaitForSeconds(0.00002f);
        }
        DealInProgress--;
    }

    public void DrawCard()
    {
        StartCoroutine(DrawCardCoroutine());
    }

    public IEnumerator DrawCardCoroutine()
    {
        DealInProgress++;
        
        Card firstDraw = _stackCardSlot.TopCard();
        if (firstDraw == null)
        {
            GameResultManager.response = CardInfo.ReturnOpenedCardInfo();
            AdsManager.Instance._InterstitialAd.LoadAd();
            yield return new WaitForSeconds(1.25f);
            CardInfo.ResetHashSet();
            CardInfo.ClearInfoPanel();
            cardNames.Clear();
            DestroyAllCards();
            isDrawFinished = true;
            yield break;
        }

        MoveCardSlotToCardSlot(_stackCardSlot, _currentCardSlot);
        yield return new WaitForSeconds(0.5f);

        if (!TryComplete11WithMultiple(_currentCardSlot.TopCard()))
        {
            Card secondDraw = _stackCardSlot.TopCard();
            if (secondDraw == null)
            {
                yield break;
            }
            MoveCardSlotToCardSlot(_stackCardSlot, _currentCardSlot);
            yield return new WaitForSeconds(0.2f);

            if (!TryComplete11WithMultiple(_currentCardSlot.TopCard()))
            {
                // Ceza: Çekilen son kartın değerinin 2 katı
                int penalty = _currentCardSlot.TopCard().FaceValue * 2;
                playerScore -= penalty;
            }
        }

        DealInProgress--;
    }

    private bool TryComplete11WithMultiple(Card newCardNewNew)
    {
        if (newCardNewNew == null) return false;

        // Tüm orta slotlardaki mevcut kartları toplayın
        List<Card> middleCards = new List<Card>();
        foreach (var slot in _middleCardSlots)
        {
            if (slot.TopCard() != null) middleCards.Add(slot.TopCard());
        }

        // Yeni kartın faceValue'u ile orta kartlardan alt kümelerle 11 kontrolü
        var combinations = GetSubsets(middleCards);
        foreach (var combo in combinations)
        {
            int sumVal = combo.Sum(c => c.FaceValue) + newCardNewNew.FaceValue;
            if (sumVal == 11)
            {
                // 11 tamamlandı, skor artır
                playerScore += 11;
                // Seçilen combo kartlarını discard slotuna gönder
                foreach (var c in combo)
                {
                    var slotOfCard = _middleCardSlots.FirstOrDefault(s => s.TopCard() == c);
                    if (slotOfCard != null)
                    {
                        slotOfCard.RemoveCard(c);
                        _discardCardSlot.AddCard(c);
                    }
                }
                // Yeni kartı da discard'a gönder
                _currentCardSlot.RemoveCard(newCardNewNew);
                _discardCardSlot.AddCard(newCardNewNew);

                // Boşalan orta slotlara yeniden kart çek
                RefillMiddleSlots();
                return true;
            }
        }
        return false;
    }

    private List<List<Card>> GetSubsets(List<Card> cards)
    {
        // Basit tüm alt kümeleri üreten örnek (çok kart olduğunda optimize etmek gerekir)
        List<List<Card>> allSubsets = new List<List<Card>>();
        int subsetCount = 1 << cards.Count;
        for (int i = 0; i < subsetCount; i++)
        {
            List<Card> subset = new List<Card>();
            for (int bit = 0; bit < cards.Count; bit++)
            {
                if ((i & (1 << bit)) != 0)
                {
                    subset.Add(cards[bit]);
                }
            }
            allSubsets.Add(subset);
        }
        return allSubsets;
    }

    private IEnumerator DealToMiddleAndCurrent()
    {
        // Deal 1 card to each middle slot
        for (int i = 0; i < _middleCardSlots.Count; i++)
        {
            if (_stackCardSlot.GetCardCount() > 0)
            {
                var topCard = _stackCardSlot.TopCard();
                _stackCardSlot.RemoveCard(topCard);
                _middleCardSlots[i].AddCard(topCard);
                yield return new WaitForSeconds(CardStackDelay);
            }
        }
        // Deal 1 card to current slot
        if (_stackCardSlot.GetCardCount() > 0)
        {
            var topCard = _stackCardSlot.TopCard();
            _stackCardSlot.RemoveCard(topCard);
            _currentCardSlot.AddCard(topCard);
        }
    }

    private void RefillMiddleSlots()
    {
        foreach (var slot in _middleCardSlots)
        {
            if (slot.GetCardCount() == 0 && _stackCardSlot.GetCardCount() > 0)
            {
                var topCard = _stackCardSlot.TopCard();
                if (topCard != null)
                {
                    _stackCardSlot.RemoveCard(topCard);
                    slot.AddCard(topCard);
                }
            }
        }
    }

    public IEnumerator ShuffleCoroutine()
    {
        DealInProgress++;
        MoveCardSlotToCardSlot(_rightHandCardSlot, _stackCardSlot);        
        MoveCardSlotToCardSlot(_leftHandCardSlot, _stackCardSlot);
        MoveCardSlotToCardSlot(_currentCardSlot, _stackCardSlot);    
        MoveCardSlotToCardSlot(_discardCardSlot, _stackCardSlot);
        foreach (var middleCard in _middleCardSlots)
        {
            MoveCardSlotToCardSlot(middleCard, _stackCardSlot);
        }
        yield return new WaitForSeconds(.4f);
        
        List<Card> shuffledDeck = new List<Card>(_stackCardSlot.CardList);
        System.Random rng = new System.Random();
        int n = shuffledDeck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = shuffledDeck[k];
            shuffledDeck[k] = shuffledDeck[n];
            shuffledDeck[n] = value;
        }

        int halfLength = shuffledDeck.Count / 2;
        for (int i = 0; i < halfLength; ++i)
        {
            _leftHandCardSlot.AddCard(shuffledDeck[i]);
        }
        yield return new WaitForSeconds(.2f);
        for (int i = halfLength; i < shuffledDeck.Count; ++i)
        {
            _rightHandCardSlot.AddCard(shuffledDeck[i]);
        }
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < shuffledDeck.Count; ++i)
        {
            if (i % 2 == 0)
            {
                _stackCardSlot.AddCard(_rightHandCardSlot.TopCard());
            }
            else
            {
                _stackCardSlot.AddCard(_leftHandCardSlot.TopCard());
            }

            yield return new WaitForSeconds(CardStackDelay);
        }
        
        yield return new WaitForSeconds(1.2f);
        yield return StartCoroutine(DealToMiddleAndCurrent());
        DealInProgress--;
    }
    
    private void DestroyAllCards()
    {
        Card[] allCards = GameObject.FindObjectsOfType<Card>();
        foreach (Card card in allCards)
        {
            // Kartın parent objesi varsa, önce onu yok et
            if (card.transform.parent != null)
            {
                Destroy(card.transform.parent.gameObject);
            }
            card.DestroyTargetObject();
            Destroy(card.gameObject);
        }
        _stackCardSlot.RemoveAllCards();
        _rightHandCardSlot.RemoveAllCards();
        _leftHandCardSlot.RemoveAllCards();
        _currentCardSlot.RemoveAllCards();

        // AssetBundle'leri temizle
        BundleSingleton.Instance.OnDestroy();
    }
}