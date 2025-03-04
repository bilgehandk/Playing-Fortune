using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour 
{
	[SerializeField]
	private GameObject _cardPrefab;
    
	[SerializeField]
	private Sprite[] cardFrontSprites;  // Holds all card face sprites

	public readonly List<Card> CardList = new List<Card>();

	public void InstantiateDeck()
	{
		for (int i = 0; i < cardFrontSprites.Length; i++)
		{
			GameObject cardInstance = Instantiate(_cardPrefab);
			Card cardNewNew = cardInstance.GetComponent<Card>();

			cardNewNew.gameObject.name = cardFrontSprites[i].name;
			cardNewNew.SetCardFrontSprite(cardFrontSprites[i]);  // Assign the card front sprite
			cardNewNew.FaceValue = StringToFaceValue(cardNewNew.gameObject.name);
			cardNewNew.FaceName = StringToFaceName(cardNewNew.gameObject.name);
			cardNewNew.FaceType = StringToType(cardNewNew.gameObject.name);

			CardList.Add(cardNewNew);
		}
	}
	
	private int StringToFaceValue(string input)
	{
		for (int i = 2; i < 11; ++i)
		{
			if (input.Contains(i.ToString()))
			{
				return i;
			}
		}
		if (input.Contains("jack") ||
		    input.Contains("queen") ||
		    input.Contains("king"))
		{
			return 10;
		}
		if (input.Contains("ace"))
		{
			return 11;
		}
		return 0;
	}

	private string StringToType(string input)
	{
		
		if (input.Contains("jack"))
		{
			return "jack";
		}
		else if(input.Contains("queen"))
		{
			return "queen";
		}
		else if(input.Contains("king"))
		{
			return "king";
		}
		else if (input.Contains("ace"))
		{
			return "ace";
		}
		else
		{
			for (int i = 2; i < 11; ++i)
			{
				if (input.Contains(i.ToString()))
				{
					return i.ToString();
				}
			}
		}
		
		return "null";
	}

	private string StringToFaceName(string input)
	{
		if (input.Contains("jack"))
		{
			if (input.Contains("clubs"))
			{
				return "jack clubs";
			}
			else if (input.Contains("diamonds"))
			{
				return "jack diamonds";
			}
			else if(input.Contains("hearts"))
			{
				return "jack hearts";
			}
			else
			{
				return "jack spades";
			}
		}
		else if (input.Contains("queen"))
		{
			if (input.Contains("clubs"))
			{
				return "queen clubs";
			}
			else if (input.Contains("diamonds"))
			{
				return "queen diamonds";
			}
			else if(input.Contains("hearts"))
			{
				return "queen hearts";
			}
			else
			{
				return "queen spades";
			}
		}
		else if (input.Contains("ace"))
		{
			if (input.Contains("clubs"))
			{
				return "ace clubs";
			}
			else if (input.Contains("diamonds"))
			{
				return "ace diamonds";
			}
			else if(input.Contains("hearts"))
			{
				return "ace hearts";
			}
			else
			{
				return "ace spades";
			}
		}
		else if (input.Contains("king"))
		{
			if (input.Contains("clubs"))
			{
				return "king clubs";
			}
			else if (input.Contains("diamonds"))
			{
				return "king diamonds";
			}
			else if(input.Contains("hearts"))
			{
				return "king hearts";
			}
			else
			{
				return "king spades";
			}
		}
		else
		{
			for (int i = 2; i < 11; ++i)
			{
				if (input.Contains(i.ToString()))
				{
					if (input.Contains("clubs"))
					{
						return i + " clubs";
					}
					else if (input.Contains("diamonds"))
					{
						return i + " diamonds";
					}
					else if(input.Contains("hearts"))
					{
						return i + " hearts";
					}
					else
					{
						return i + " spades";
					}
				}
			}
		}

		return null;
	}
}
