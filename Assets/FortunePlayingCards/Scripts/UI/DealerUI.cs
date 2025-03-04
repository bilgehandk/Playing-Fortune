using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class DealerUI : MonoBehaviour 
{
    private Dealer _dealer;
    private bool isShuffled = false;
    public Button ShuffleButton;
    public Button TellFortuneButton;
    public Button GoMainPageButton;
    public TMP_Text score;
    public TMP_Text scoreNum;
    public TMP_Text winGameResult;
    public TMP_Text loseGameResult;
    private LocalizedString _localizedString;
    
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _dealer = GameObject.Find("Dealer").GetComponent<Dealer>();
        _dealer.DealerUIInstance = this;

        _dealer.CardInfo = gameObject.GetComponentInChildren<CardInfo>(); // CardInfo bileşenini bu şekilde alın
        if (_dealer.CardInfo == null)
        {
            Debug.LogError("CardInfo bileşeni bulunamadı!");
        }
        AdsManager.Instance._BannerAdExample.ShowBannerAd();
    }

    private void Start()
    {
        TellFortuneButton.onClick.AddListener(OpenFortune);
        GoMainPageButton.onClick.AddListener(GoMainPage);
    }
    
    private void GoMainPage()
    {
        GameManager.Instance.LoadScene(SceneType.Entrance);
    }

    private void OpenFortune()
    {
        GameManager.Instance.LoadScene(SceneType.GameResult);
    }

    private void Update()
    {
        if (isShuffled)
        {
            Draw();
        }

        scoreNum.text = _dealer.score.ToString();

        if (_dealer.isDrawFinished)
        {
            score.gameObject.SetActive(false);
            scoreNum.gameObject.SetActive(false);
            if (_dealer.score > 0)
                TellFortuneButton.gameObject.SetActive(true);
            GoMainPageButton.gameObject.SetActive(true);
            
            if (_dealer.score >= 40)
                winGameResult.gameObject.SetActive(true);
            else
                loseGameResult.gameObject.SetActive(true);
        }
    }

    public void Shuffle()
    {
        if (_dealer.DealInProgress == 0)
        {
            StartCoroutine(_dealer.ShuffleCoroutine());
        }
        ShuffleButton.gameObject.SetActive(false);
        isShuffled = true;
        score.gameObject.SetActive(true);
        scoreNum.gameObject.SetActive(true);
    }
    
    public void Draw()
    {
        if (_dealer.DealInProgress == 0)
        {
            StartCoroutine(_dealer.DrawCoroutine());
        }
    }
}