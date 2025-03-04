using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private Sprite[] _CardSprites;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform infoPanel;

    private List<string> _cardNames;
    private HashSet<string> _instantiatedCardNames; // Yeni eklenen liste
    public string OpenedCards;

    private void Awake()
    {
        _instantiatedCardNames = new HashSet<string>();
    }

    public bool AddCardToInfoPanel(Sprite cardSprite)
    {
        if (cardPrefab == null || infoPanel == null)
        {
            Debug.LogError("cardPrefab veya infoPanel referansı atanmış değil.");
            return false;
        }

        GameObject newCard = Instantiate(cardPrefab, infoPanel);
        if (newCard != null)
        {
            Image cardImage = newCard.GetComponent<Image>();
            if (cardImage != null)
            {
                cardImage.sprite = cardSprite;
                newCard.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(50, 80);
                return true;
            }
            else
            {
                Debug.LogError("cardPrefab GameObject'i Image bileşenine sahip değil.");
                return false;
            }
        }

        return false;
    }

    public IEnumerator UpdateCard(List<string> _cards)
    {
        if (_cards == null || _cards.Count == 0)
        {
            Debug.LogError("_cards dizisi boş veya null.");
            yield break;
        }

        _cardNames = _cards;

        foreach (var cardName in _cardNames)
        {
            if (string.IsNullOrEmpty(cardName))
            {
                Debug.LogError($"_cardNames içindeki değer null veya boş: {cardName}");
                continue;
            }

            if (_instantiatedCardNames.Contains(cardName))
            {
                // Kart daha önce instantiate edilmiş, bu nedenle tekrar instantiate etmeye gerek yok.
                continue;
            }

            Sprite matchedSprite = null;
            for (int j = 0; j < _CardSprites.Length; j++)
            {
                if (_CardSprites[j] == null || string.IsNullOrEmpty(_CardSprites[j].name))
                {
                    Debug.LogError($"_CardSprites[{j}] veya ismi null veya boş.");
                    continue;
                }

                if (_CardSprites[j].name == cardName)
                {
                    matchedSprite = _CardSprites[j];
                    break;
                }
            }

            if (matchedSprite != null)
            {
                bool added = AddCardToInfoPanel(matchedSprite);
                if (added)
                {
                    // Kart başarılı bir şekilde instantiate edildikten sonra ismi listeye eklenir.
                    _instantiatedCardNames.Add(cardName);
                }
                else
                {
                    Debug.LogError($"Kart eklenirken bir hata oluştu: {cardName}");
                }
            }
            else
            {
                Debug.LogError($"Eşleşen sprite bulunamadı: {cardName}");
            }
        }
    }

    public void ResetHashSet()
    {
        _instantiatedCardNames.Clear();
    }
    
    public void ClearInfoPanel()
    {
        _CardSprites = new Sprite[0];
    }

    public string ReturnOpenedCardInfo()
    {
        int i = 0;
        foreach (var instantiatedCardName in _instantiatedCardNames)
        {
            i++;
            OpenedCards += ($"{i}. card = " + instantiatedCardName + "\n");
        }

        return OpenedCards;
    }
}
