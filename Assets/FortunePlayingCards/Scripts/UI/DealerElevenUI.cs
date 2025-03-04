using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Localization;

namespace FortunePlayingCards.Scripts.UI
{
    public class DealerElevenUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DealerEleven _dealerEleven;
        
        [Header("UI Elements")]
        [SerializeField] private Button _shuffleButton;
        [SerializeField] private Button _goMainPageButton;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _scoreNum;
        [SerializeField] private TMP_Text _winGameResult;
        [SerializeField] private TMP_Text _loseGameResult;

        private bool _isShuffled = false;
        private LocalizedString _localizedString;

        private void Awake()
        {
            InitializeComponents();
            SetupUI();
            InitializeGameSettings();
            
        }

        private void InitializeComponents()
        {
            // Try to get DealerEleven from inspector first
            if (_dealerEleven == null)
            {
                // If not assigned, try to find in scene
                _dealerEleven = GameObject.FindObjectOfType<DealerEleven>();
                
                if (_dealerEleven == null)
                {
                    Debug.LogError("DealerEleven component is missing! Please assign it in the inspector or ensure it exists in the scene.");
                    enabled = false; // Disable this component if DealerEleven is missing
                    return;
                }
            }

            _dealerEleven.DealerUIInstance = this;
            
            // Initialize CardInfo
            var cardInfo = gameObject.GetComponentInChildren<CardInfo>();
            if (cardInfo == null)
            {
                Debug.LogWarning("CardInfo component not found in children!");
            }
            _dealerEleven.CardInfo = cardInfo;
        }

        private void SetupUI()
        {
            ValidateUIComponents();
            
            if (_shuffleButton != null)
                _shuffleButton.onClick.AddListener(Shuffle);
            
            if (_goMainPageButton != null)
                _goMainPageButton.onClick.AddListener(GoMainPage);
        }

        private void ValidateUIComponents()
        {
            if (_shuffleButton == null)
                Debug.LogError("Shuffle Button is not assigned!");
            if (_goMainPageButton == null)
                Debug.LogError("Go Main Page Button is not assigned!");
            if (_score == null)
                Debug.LogError("Score Text is not assigned!");
            if (_scoreNum == null)
                Debug.LogError("Score Number Text is not assigned!");
            if (_winGameResult == null)
                Debug.LogError("Win Game Result Text is not assigned!");
            if (_loseGameResult == null)
                Debug.LogError("Lose Game Result Text is not assigned!");
        }

        private void InitializeGameSettings()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            _dealerEleven.DealInProgress = 0;

            // Initialize UI state
            if (_score != null) _score.gameObject.SetActive(false);
            if (_scoreNum != null) _scoreNum.gameObject.SetActive(false);
            if (_goMainPageButton != null) _goMainPageButton.gameObject.SetActive(false);
            if (_winGameResult != null) _winGameResult.gameObject.SetActive(false);
            if (_loseGameResult != null) _loseGameResult.gameObject.SetActive(false);

            // Setup ads if available
            if (AdsManager.Instance != null && AdsManager.Instance._BannerAdExample != null)
            {
                AdsManager.Instance._BannerAdExample.ShowBannerAd();
            }
        }

        private void Update()
        {
            if (_dealerEleven == null) return;

            UpdateScore();
            CheckGameEnd();
        }

        private void UpdateScore()
        {
            if (_scoreNum != null)
            {
                _scoreNum.text = _dealerEleven.playerScore.ToString();
            }
        }

        private void CheckGameEnd()
        {
            if (!_dealerEleven.isDrawFinished) return;

            // Hide score display
            if (_score != null) _score.gameObject.SetActive(false);
            if (_scoreNum != null) _scoreNum.gameObject.SetActive(false);
            
            // Show main page button
            if (_goMainPageButton != null) _goMainPageButton.gameObject.SetActive(true);

            // Show appropriate result
            if (_dealerEleven.playerScore > 0)
            {
                if (_winGameResult != null) _winGameResult.gameObject.SetActive(true);
            }
            else
            {
                if (_loseGameResult != null) _loseGameResult.gameObject.SetActive(true);
            }
        }

        public void Shuffle()
        {
            if (_dealerEleven.DealInProgress == 0)
                StartCoroutine(_dealerEleven.ShuffleCoroutine());
        }

        public void Draw()
        {
            if (_dealerEleven.DealInProgress == 0)
                StartCoroutine(_dealerEleven.DrawCardCoroutine());
        }

        private void GoMainPage()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadScene(SceneType.Entrance);
            }
            else
            {
                Debug.LogError("GameManager instance is missing!");
            }
        }

        // Optional: OnValidate to help setup in editor
        private void OnValidate()
        {
            if (_dealerEleven == null)
            {
                _dealerEleven = GetComponent<DealerEleven>();
            }
        }
    }
}