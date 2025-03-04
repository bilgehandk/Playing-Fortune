using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardSlot : MonoBehaviour 
{
    public readonly List<Card> CardList = new List<Card>();

    [SerializeField]
    private bool _inverseStack;

    [Range(0.05f, 0.3f)]
    [SerializeField]
    private float _positionDamp = .2f;

    [Range(0.05f, 0.3f)]
    [SerializeField] 
    private float _rotationDamp = .2f;   
    
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    
    public int FaceValue()
    {
        int collectiveFaceValue = 0;
        for (int i = 0; i < CardList.Count; ++i)
        {
            collectiveFaceValue += CardList[i].FaceValue;
        }
        return collectiveFaceValue;
    }
    
    public Card TopCard()
    {
        if (CardList.Count > 0)
        {
            return CardList[CardList.Count - 1];
        }
        else
        {
            return null;
        }   
    }
    
    public Card BottomCard()
    {
        if (CardList.Count > 0)
        {
            return CardList[0];
        }
        else
        {
            return null;
        }           
    }
    
    public bool AddCard(Card cardNewNew)
    {
        if (cardNewNew != null)
        {
            if (cardNewNew.ParentCardSlot != null)
            {
                cardNewNew.ParentCardSlot.RemoveCard(cardNewNew);
            }
            cardNewNew.ParentCardSlot = this;
            CardList.Add(cardNewNew);

            // Kartın pozisyon ve rotasyonunu ayarlıyoruz
            cardNewNew.TargetTransform.rotation = transform.rotation;

            float cardHeight = cardNewNew.GetComponent<BoxCollider>().size.z;
            float cardThickness = cardNewNew.GetComponent<BoxCollider>().size.y;

            // Yeni kartın pozisyonunu mevcut kartların yüksekliğine ve kalınlığına göre ayarlıyoruz
            Vector3 newPosition = transform.position;

            if (_inverseStack)
            {
                newPosition.y += CardList.Count * cardThickness * 0.012f; // Y eksenindeki mesafeyi azaltmak için 0.012 ile çarpıyoruz
                newPosition.z -= CardList.Count * cardHeight * 0.012f; // Z eksenindeki mesafeyi azaltmak için 0.012 ile çarpıyoruz
            }
            else
            {
                newPosition.y += CardList.Count * cardThickness * 0.012f; // Y eksenindeki mesafeyi azaltmak için 0.012 ile çarpıyoruz
                newPosition.z += CardList.Count * cardHeight * 0.012f; // Z eksenindeki mesafeyi azaltmak için 0.012 ile çarpıyoruz
            }

            cardNewNew.TargetTransform.position = newPosition;

            cardNewNew.SetDamp(_positionDamp, _rotationDamp);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveCard(Card cardNewNew)
    {
        cardNewNew.ParentCardSlot = null;
        CardList.Remove(cardNewNew);
    }
    
    public void RemoveAllCards()
    {
        foreach (Card card in CardList.ToList())
        {
            RemoveCard(card);
            Destroy(card.gameObject); // Kartın GameObject'ini yok et
        }
    }

}
