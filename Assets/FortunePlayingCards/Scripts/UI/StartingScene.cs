using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class StartingScene : MonoBehaviour
{
    [SerializeField] public Button RewardAdButton;
    [SerializeField] public Button ShowGameScene;
    [SerializeField] public Button SettingsScene;
    [SerializeField] public Button InfoPanelButton;
    [SerializeField] public Button ResetGameButton;
    [SerializeField] public Button CloseInfoPanelButton;
    [SerializeField] public GameObject InfoPanel;
    [SerializeField] public TMP_Text remainingRights;
    [SerializeField] public TMP_Text header;
    


    private void Start()
    {
        AdsManager.Instance._RewardedAds.LoadAd();
        SettingsScene.onClick.AddListener(OpenSettingScene);
        InfoPanelButton.onClick.AddListener(OpenInfoPanel);
        ResetGameButton.onClick.AddListener(ResetGamePlayer);
        CloseInfoPanelButton.onClick.AddListener(CloseInfoPanel);
    }

    private void CloseInfoPanel()
    {
        InfoPanel.SetActive(false);
        if (AdsManager.Instance.IsRewardedAdReady())
        {
            RewardAdButton.gameObject.SetActive(true);
        }
        else
            ShowGameScene.gameObject.SetActive(true);
        SettingsScene.gameObject.SetActive(true);
        InfoPanelButton.gameObject.SetActive(true);
        remainingRights.gameObject.SetActive(true);
        header.gameObject.SetActive(true);
    }

    private void ResetGamePlayer()
    {
        PlayerPrefs.DeleteKey("name");
        PlayerPrefs.DeleteKey("age");
        PlayerPrefs.DeleteKey("gender");
        PlayerPrefs.DeleteKey("relationship");
        GameManager.Instance.LoadScene(SceneType.UserInFo);
    }

    private void OpenInfoPanel()
    {
        InfoPanel.SetActive(true);
        RewardAdButton.gameObject.SetActive(false);
        ShowGameScene.gameObject.SetActive(false);
        SettingsScene.gameObject.SetActive(false);
        InfoPanelButton.gameObject.SetActive(false);
        remainingRights.gameObject.SetActive(false);
        header.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Remaining rights'i güncelle
        int remaining = PlayerPrefs.GetInt("isFirstTime");
        remainingRights.text = remaining + "/3";

        // Kalan hakları kontrol et
        if (remaining <= 0 && AdsManager.Instance.IsRewardedAdReady())
        {
            RewardAdButton.gameObject.SetActive(true);
            ShowGameScene.gameObject.SetActive(false);
        }
        else if(remaining <= 0)
        {
            RewardAdButton.gameObject.SetActive(true);
            ShowGameScene.gameObject.SetActive(false);
        }
        else
        {
            RewardAdButton.gameObject.SetActive(false);
            ShowGameScene.gameObject.SetActive(true);
        }

        // Reklam izlenmişse
        if (GameManager.Instance.isAdRewardedOpened)
        {
            PlayerPrefs.SetInt("isFirstTime", GameManager.Instance.remaningRights + 1);
            GameManager.Instance.isAdRewardedOpened = false;  // Bu durumu sıfırla
        }

        if (Application.internetReachability==new NetworkReachability())
        {
            GameManager.Instance.LoadScene(SceneType.InternetCheck);
        }
    }


    public void ShowRewardedAd()
    {
        AdsManager.Instance._BannerAdExample.HideBannerAd();

        // Reklamın yüklü olup olmadığını kontrol et
        if (AdsManager.Instance.IsRewardedAdReady())
        {
            AdsManager.Instance._RewardedAds.ShowAd();
        }
        else
        {
            AdsManager.Instance._RewardedAds.LoadAd();
            Debug.Log("Reklam henüz yüklenmedi.");
            
            StartCoroutine(WaitForAdToLoad());
        }
    }
    
    IEnumerator WaitForAdToLoad()
    {
        // Reklam yüklenene kadar bekle
        while (!AdsManager.Instance._RewardedAds.IsLoaded())
        {
            yield return null;  // Her frame'de kontrol et
        }

        // Reklam yüklendiğinde butonu aktif hale getir
        Debug.Log("Ad is loaded. You can now show the ad.");
        AdsManager.Instance._RewardedAds.ShowAd();
    }


    public void OpenSettingScene()
    {
        GameManager.Instance.LoadScene(SceneType.Settings);
    }

    public void StartGame()
    {
        GameManager.Instance.LoadScene(SceneType.Loading);
        PlayerPrefs.SetInt("isFirstTime", GameManager.Instance.remaningRights-1);
    }
}
