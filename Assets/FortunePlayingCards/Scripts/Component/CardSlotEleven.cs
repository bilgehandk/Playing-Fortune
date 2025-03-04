using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FortunePlayingCards.Scripts.Component
{
    public class CardSlotEleven : MonoBehaviour
    {
        public readonly List<Card> CardList = new List<Card>();

        [Header("Stack Settings")]
        [SerializeField] private bool _inverseStack;
        [Range(0.05f, 0.3f)] [SerializeField] private float _positionDamp = .2f;
        [SerializeField] private bool _stackCardSlot = false;
        [Range(0.05f, 0.3f)] [SerializeField] private float _rotationDamp = .2f;

        [Header("Card Positioning")]
        [SerializeField] private float _cardSpacingMultiplier = 0.012f;

        private void Awake()
        {
            InitializeMeshRenderer();
        }

        private void InitializeMeshRenderer()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        public int FaceValue()
        {
            return CardList.Sum(card => card.FaceValue);
        }

        public Card TopCard()
        {
            return CardList.Count > 0 ? CardList[^1] : null;
        }

        public Card BottomCard()
        {
            return CardList.Count > 0 ? CardList[0] : null;
        }

        public bool AddCard(Card cardNewNew)
        {
            if (cardNewNew == null) return false;
            // Check sum limit
            if (FaceValue() + cardNewNew.FaceValue > 11 && !_stackCardSlot) 
            {
                Debug.LogWarning("Cannot add card. Would exceed 11 in this slot.");
                return false;
            }
            cardNewNew.ParentCardSlotEleven?.RemoveCard(cardNewNew);
            cardNewNew.ParentCardSlotEleven = this;
            CardList.Add(cardNewNew);
            UpdateCardTransform(cardNewNew);
            return true;
        }

        private void UpdateCardTransform(Card cardNewNew)
        {
            if (cardNewNew == null)
                return;

            // Set rotation
            cardNewNew.TargetTransform.rotation = transform.rotation;

            // Get card dimensions
            var boxCollider = cardNewNew.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                Debug.LogError($"BoxCollider missing on card: {cardNewNew.name}");
                return;
            }

            float cardHeight = boxCollider.size.z;
            float cardThickness = boxCollider.size.y;

            // Calculate new position
            Vector3 newPosition = CalculateCardPosition(cardHeight, cardThickness);
            cardNewNew.TargetTransform.position = newPosition;

            // Set damping values
            cardNewNew.SetDamp(_positionDamp, _rotationDamp);
        }

        private Vector3 CalculateCardPosition(float cardHeight, float cardThickness)
        {
            Vector3 newPosition = transform.position;
            float heightOffset = CardList.Count * cardThickness * _cardSpacingMultiplier;
            float depthOffset = CardList.Count * cardHeight * _cardSpacingMultiplier;

            newPosition.y += heightOffset;
            
            if (_inverseStack)
                newPosition.z -= depthOffset;
            else
                newPosition.z += depthOffset;

            return newPosition;
        }

        public void RemoveCard(Card cardNewNew)
        {
            if (cardNewNew == null)
                return;

            cardNewNew.ParentCardSlotEleven = null;
            CardList.Remove(cardNewNew);
        }

        public void RemoveAllCards()
        {
            // Create a copy of the list to avoid modification during enumeration
            var cardsToRemove = CardList.ToList();
            
            foreach (var card in cardsToRemove)
            {
                if (card != null)
                {
                    RemoveCard(card);
                    Destroy(card.gameObject);
                }
            }
        }

        // Helper method to check if slot contains specific card
        public bool ContainsCard(Card cardNewNew)
        {
            return cardNewNew != null && CardList.Contains(cardNewNew);
        }

        // Helper method to get card count
        public int GetCardCount()
        {
            return CardList.Count;
        }

        // Optional: Validate components in editor
        private void OnValidate()
        {
            if (_cardSpacingMultiplier <= 0)
            {
                Debug.LogWarning("Card spacing multiplier should be greater than 0");
                _cardSpacingMultiplier = 0.012f;
            }
        }

        // Optional: Debug visualization
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.1f);
        }
    }
}