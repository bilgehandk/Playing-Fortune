using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Dealer : MonoBehaviour 
{
    public DealerUI DealerUIInstance { get; set; }
    
    [SerializeField]
    private CardDeck _cardDeck;    

    [SerializeField]
    private CardSlot _pickupCardSlot;        

    [SerializeField]
    private CardSlot _stackCardSlot;    

    [SerializeField]
    private CardSlot _discardStackCardSlot;        

    [SerializeField]
    private CardSlot _discardHoverStackCardSlot;            

    [SerializeField]
    private CardSlot _rightHandCardSlot;

    [SerializeField]
    private CardSlot _leftHandCardSlot;

    [SerializeField]
    private CardSlot _currentCardSlot;    
    
    [SerializeField]
    private CardSlot _currentCardSlot1;    

    [SerializeField]
    private CardSlot _prior0CardSlot;    

    [SerializeField]
    private CardSlot _prior1CardSlot;    
    
    [SerializeField]
    private CardSlot _fortuneCardSlot0;    

    [SerializeField]
    private CardSlot _fortuneCardSlot1;

    public static List<string> cardNames = new List<string>();

    private const float CardStackDelay = .01f;

    public CardInfo CardInfo;

    public bool isDrawFinished = false;
    public int score = 0;
    
    /// <summary>
    /// Counter which keeps track current dealing movements in progress.
    /// </summary>
    public int DealInProgress { get; set; }

    private void Awake()
    {
        _cardDeck.InstantiateDeck();
        StartCoroutine(StackCardRangeOnSlot(0, _cardDeck.CardList.Count, _stackCardSlot));
    }

    private void MoveCardSlotToCardSlot(CardSlot sourceCardSlot, CardSlot targetCardSlot) 
    {
        Card cardNewNew;
        while ((cardNewNew = sourceCardSlot.TopCard()) != null)
        {
            targetCardSlot.AddCard(cardNewNew);
        }
    }
    
    private IEnumerator StackCardRangeOnSlot(int start, int end, CardSlot cardSlot) 
    {
        DealInProgress++;
        for (int i = start; i < end; ++i)
        {
            cardSlot.AddCard(_cardDeck.CardList[i]);
            yield return new WaitForSeconds(0.00002f);
        }
        DealInProgress--;
    }
    
    public IEnumerator ShuffleCoroutine()
    {
        DealInProgress++;
        MoveCardSlotToCardSlot(_stackCardSlot, _pickupCardSlot);        
        MoveCardSlotToCardSlot(_prior0CardSlot, _pickupCardSlot);
        MoveCardSlotToCardSlot(_prior1CardSlot, _pickupCardSlot);    
        MoveCardSlotToCardSlot(_discardStackCardSlot, _pickupCardSlot);
        MoveCardSlotToCardSlot(_currentCardSlot, _pickupCardSlot);
        MoveCardSlotToCardSlot(_currentCardSlot1, _pickupCardSlot);
        MoveCardSlotToCardSlot(_fortuneCardSlot0, _pickupCardSlot);
        MoveCardSlotToCardSlot(_fortuneCardSlot1, _pickupCardSlot);
        yield return new WaitForSeconds(.4f);

        // Kartları karıştırmak için bir liste oluştur ve karıştır
        List<Card> shuffledDeck = new List<Card>(_pickupCardSlot.CardList);
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

        // Karışık kartları iki ele böl
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

        // Kartları sırasıyla tekrar stack'e ekle
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

        yield return new WaitForSeconds(1.25f);
        DealInProgress--;
    }

    public IEnumerator DrawCoroutine()
    {
        DealInProgress++;

        if (_currentCardSlot == null || _stackCardSlot == null || _currentCardSlot1 == null ||
            _prior0CardSlot == null || _prior1CardSlot == null || _fortuneCardSlot0 == null ||
            _fortuneCardSlot1 == null || _discardHoverStackCardSlot == null || _discardStackCardSlot == null)
        {
            Debug.LogError("One or more CardSlot references are not set in the Dealer component.");
            yield break;
        }

        if (_currentCardSlot.AddCard(_stackCardSlot.TopCard()) && _currentCardSlot1.AddCard(_stackCardSlot.TopCard()))
        {
            yield return new WaitForSeconds(0.5f);
            if (_prior0CardSlot.AddCard(_currentCardSlot.TopCard()) && _prior1CardSlot.AddCard(_currentCardSlot1.TopCard()))
            {
                yield return new WaitForSeconds(0.5f);
                if (_prior0CardSlot.TopCard().FaceType == _prior1CardSlot.TopCard().FaceType)
                {
                    yield return new WaitForSeconds(0.5f);
                    if (_fortuneCardSlot1.AddCard(_prior1CardSlot.TopCard()) &&
                        _fortuneCardSlot0.AddCard(_prior0CardSlot.TopCard()))
                    {
                        yield return new WaitForSeconds(CardStackDelay);
                        
                        // cardNames listesini güncelle
                        Debug.Log("Kart 1: " + _fortuneCardSlot0.TopCard().FaceName);
                        Debug.Log("Kart 2: " + _fortuneCardSlot1.TopCard().FaceName);
                        score += _fortuneCardSlot0.TopCard().FaceValue + _fortuneCardSlot1.TopCard().FaceValue;
                        cardNames.Add(_fortuneCardSlot0.TopCard().FaceName);
                        cardNames.Add(_fortuneCardSlot1.TopCard().FaceName);
                        
                        yield return new WaitForSeconds(0.05f);
                        StartCoroutine(CardInfo.UpdateCard(cardNames)); // Listeyi diziye çevirerek gönder
                    }
                    
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    if (_discardHoverStackCardSlot.AddCard(_prior0CardSlot.TopCard()) &&
                        _discardHoverStackCardSlot.AddCard(_prior1CardSlot.TopCard()))
                    {
                        yield return new WaitForSeconds(CardStackDelay);
                    }
                }
            }
        }

        if (_discardStackCardSlot.AddCard(_discardHoverStackCardSlot.TopCard()) && _discardStackCardSlot.AddCard(_discardHoverStackCardSlot.TopCard()))
        {
            yield return new WaitForSeconds(CardStackDelay);
        }

        if (_stackCardSlot.TopCard() == null)
        {
            Debug.Log(CardInfo.ReturnOpenedCardInfo());
            GameResultManager.response = CardInfo.ReturnOpenedCardInfo();
            AdsManager.Instance._InterstitialAd.LoadAd();
            yield return new WaitForSeconds(1.25f);
            CardInfo.ResetHashSet();
            CardInfo.ClearInfoPanel();
            cardNames.Clear();
            DestroyAllCards(); // Sadece kartları yok et
            isDrawFinished = true;
            yield break;
        }
        
        yield return new WaitForSeconds(1f);
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
        _pickupCardSlot.RemoveAllCards();
        _stackCardSlot.RemoveAllCards();
        _discardStackCardSlot.RemoveAllCards();
        _discardHoverStackCardSlot.RemoveAllCards();
        _rightHandCardSlot.RemoveAllCards();
        _leftHandCardSlot.RemoveAllCards();
        _currentCardSlot.RemoveAllCards();
        _currentCardSlot1.RemoveAllCards();
        _prior0CardSlot.RemoveAllCards();
        _prior1CardSlot.RemoveAllCards();
        _fortuneCardSlot0.RemoveAllCards();
        _fortuneCardSlot1.RemoveAllCards();

        // AssetBundle'leri temizle
        BundleSingleton.Instance.OnDestroy();
    }



}
