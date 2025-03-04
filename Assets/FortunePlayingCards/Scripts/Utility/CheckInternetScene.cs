using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckInternetScene : MonoBehaviour
{
    public Button tryInternetConnectionButton;

    void OnEnable()
    {
        if (GameManager.Instance.activeSceneType == SceneType.Entrance || GameManager.Instance.activeSceneType == SceneType.UserInFo)
            tryInternetConnectionButton.onClick.AddListener(CheckInternetStartGame);
        else if(GameManager.Instance.activeSceneType == SceneType.Game || GameManager.Instance.activeSceneType == SceneType.GameResult)
            tryInternetConnectionButton.onClick.AddListener(CheckInternetGameResultGame);
        
    }

    void OnDisable()
    {
        if (GameManager.Instance.activeSceneType == SceneType.Entrance || GameManager.Instance.activeSceneType == SceneType.UserInFo)
            tryInternetConnectionButton.onClick.AddListener(CheckInternetStartGame);
        else if(GameManager.Instance.activeSceneType == SceneType.Game || GameManager.Instance.activeSceneType == SceneType.GameResult)
            tryInternetConnectionButton.onClick.AddListener(CheckInternetGameResultGame);
    }

    private void CheckInternetStartGame()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            AdsManager.Instance.InitializeLoadAds();
            GameManager.Instance.StartGameAfterInternetAvaliability();
        }
    }
    
    private void CheckInternetGameResultGame()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            AdsManager.Instance.InitializeLoadAds();
            GameManager.Instance.GameResultAfterInternetAvaliability();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            GameManager.Instance.StartGameAfterInternetAvaliability();
            AdsManager.Instance.InitializeLoadAds();
        }
    }
}