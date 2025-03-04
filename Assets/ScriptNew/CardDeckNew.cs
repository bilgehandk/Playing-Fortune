using UnityEngine;
using System.Collections.Generic;

public class CardDeckNew : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardPrefab; // Must have CardNew attached

    [SerializeField]
    private Sprite[] cardFrontSprites;

    public List<CardNew> CardList = new List<CardNew>();
    public static CardDeckNew Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InstantiateDeck()
    {
        for (int i = 0; i < cardFrontSprites.Length; i++)
        {
            GameObject cardInstance = Instantiate(_cardPrefab);
            CardNew cardNew = cardInstance.GetComponent<CardNew>();

            cardNew.name = cardFrontSprites[i].name;
            cardNew.SetupCard(
                cardFrontSprites[i],
                StringToFaceValue(cardNew.name),
                StringToFaceName(cardNew.name),
                StringToType(cardNew.name)
            );
            CardList.Add(cardNew);
        }
        ShuffleDeck();
    }

    private void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = CardList.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardNew value = CardList[k];
            CardList[k] = CardList[n];
            CardList[n] = value;
        }
    }

    private int StringToFaceValue(string input)
    {
        for (int i = 2; i < 11; ++i)
        {
            if (input.Contains(i.ToString()))
                return i;
        }
        if (input.Contains("jack") || input.Contains("queen") || input.Contains("king"))
            return 10;
        if (input.Contains("ace"))
            return 1;
        return 0;
    }

    private string StringToType(string input)
    {
        if (input.Contains("jack"))      return "jack";
        else if (input.Contains("queen"))return "queen";
        else if (input.Contains("king")) return "king";
        else if (input.Contains("ace"))  return "ace";
        for (int i = 2; i < 11; ++i)
        {
            if (input.Contains(i.ToString()))
                return i.ToString();
        }
        return "null";
    }

    private string StringToFaceName(string input)
    {
        if (input.Contains("jack"))
        {
            if (input.Contains("clubs"))    return "jack clubs";
            else if (input.Contains("diamonds")) return "jack diamonds";
            else if (input.Contains("hearts"))   return "jack hearts";
            else return "jack spades";
        }
        for (int i = 2; i < 11; ++i)
        {
            if (input.Contains(i.ToString()))
            {
                if (input.Contains("clubs"))    return i + " clubs";
                else if (input.Contains("diamonds")) return i + " diamonds";
                else if (input.Contains("hearts"))   return i + " hearts";
                else return i + " spades";
            }
        }
        return null;
    }
}