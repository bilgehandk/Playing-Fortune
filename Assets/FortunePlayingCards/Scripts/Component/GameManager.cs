using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public GameObject entranceScenePrefab;
    public GameObject userInfoScenePrefab;
    public GameObject loadingScenePrefab;
    public GameObject gameScenePrefab;
    public GameObject settingsScenePrefab;
    public GameObject gameResultScenePrefab;
    public GameObject InternetCheckScenePrefab;
    public GameObject elevenScenePrefab;

    public Transform sceneParent; // Parent GameObject referansı

    private GameObject activeScene;
    public static GameManager Instance { get; private set; }
    public SceneType activeSceneType;

    private int firstRemainingRights = 3;
    public int remaningRights;

    public bool isAdRewardedOpened;
    public bool isIntersitialAdOpened;

    private bool IsSoundActive;

    // Audio yönetimi için
    public AudioClip backgroundMusic; // Arka planda çalacak müzik
    public AudioSource audioSource;  // Ses kaynağı

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        audioSource.clip = backgroundMusic;
        
        // PlayerPrefs'ten IsSoundActive durumunu yükle
        if (PlayerPrefs.HasKey("IsSoundActive"))
        {
            IsSoundActive = PlayerPrefs.GetInt("IsSoundActive") == 1;
        }
        else
        {
            // Eğer kayıtlı bir ayar yoksa, varsayılan olarak müzik açık olsun
            IsSoundActive = true;
            PlayerPrefs.SetInt("IsSoundActive", 1); // 1 = açık
        }

        if (Application.internetReachability==NetworkReachability.NotReachable)
        {
            LoadScene(SceneType.InternetCheck);
        }
        else
        {
            if (PlayerPrefs.HasKey("name"))
                LoadScene(SceneType.Entrance);
            else
                LoadScene(SceneType.UserInFo);

            StartCoroutine(ShowBanner());
            
            // Eğer ses aktifse müziği başlat
            ToggleBackgroundMusic(IsSoundActive);
        }
    }

    public void StartGameAfterInternetAvaliability()
    {
        if (Application.internetReachability!=NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.HasKey("name"))
                LoadScene(SceneType.Entrance);
            else
                LoadScene(SceneType.UserInFo);

            StartCoroutine(ShowBanner());
            
            // Eğer ses aktifse müziği başlat
            ToggleBackgroundMusic(IsSoundActive);
        }
    }
    
    public void GameResultAfterInternetAvaliability()
    {
        if (Application.internetReachability!=NetworkReachability.NotReachable)
        {
            LoadScene(SceneType.GameResult);

            StartCoroutine(ShowBanner());
            
            // Eğer ses aktifse müziği başlat
            ToggleBackgroundMusic(IsSoundActive);
        }
    }

    public IEnumerator ShowBanner()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.Instance._BannerAdExample.ShowBannerAd();
    }

    public void LoadScene(SceneType sceneType)
    {
        if (Instance == null)
        {
            Debug.LogError("GameManager instance is null");
            return;
        }

        if (Instance.activeScene != null)
        {
            Destroy(Instance.activeScene);
        }

        if (!PlayerPrefs.HasKey("isFirstTime"))
        {
            remaningRights = firstRemainingRights;
            PlayerPrefs.SetInt("isFirstTime", remaningRights);
        }
        else
        {
            remaningRights = PlayerPrefs.GetInt("isFirstTime");
        }

        if (sceneType == SceneType.Entrance || sceneType == SceneType.UserInFo || sceneType == SceneType.Game || sceneType == SceneType.GameResult || sceneType == SceneType.Eleven || sceneType == SceneType.Settings || sceneType == SceneType.InternetCheck || sceneType == SceneType.Loading)
            activeSceneType = sceneType;

        // Prefab'ları yükleyip sahneyi kur
        switch (sceneType)
        {
            case SceneType.Entrance:
                Instance.activeScene = Instantiate(Instance.entranceScenePrefab, Instance.sceneParent);
                break;
            case SceneType.UserInFo:
                Instance.activeScene = Instantiate(Instance.userInfoScenePrefab, Instance.sceneParent);
                break;
            case SceneType.Loading:
                Instance.activeScene = Instantiate(Instance.loadingScenePrefab, Instance.sceneParent);
                break;
            case SceneType.Game:
                Instance.activeScene = Instantiate(Instance.gameScenePrefab, Instance.sceneParent);
                break;
            case SceneType.Settings:
                Instance.activeScene = Instantiate(Instance.settingsScenePrefab, Instance.sceneParent);
                break;
            case SceneType.GameResult:
                Instance.activeScene = Instantiate(Instance.gameResultScenePrefab, Instance.sceneParent);
                break;
            case SceneType.InternetCheck:
                Instance.activeScene = Instantiate(Instance.InternetCheckScenePrefab, Instance.sceneParent);
                break;
            case SceneType.Eleven:
                Instance.activeScene = Instantiate(Instance.elevenScenePrefab, Instance.sceneParent);
                break;
        }

        Instance.activeScene.SetActive(true);
    }

    public void Restart()
    {
        LoadScene(SceneType.Entrance);
    }

    public IEnumerator LoadEntranceSceneAfterAd()
    {
        yield return new WaitUntil(() => GameManager.Instance.isIntersitialAdOpened);
        LoadScene(SceneType.Entrance);
    }

    // Ses açma/kapama işlevi
    public void ToggleBackgroundMusic(bool isSoundActive)
    {
        IsSoundActive = isSoundActive;

        // PlayerPrefs'e güncel ses ayarını kaydet
        PlayerPrefs.SetInt("IsSoundActive", IsSoundActive ? 1 : 0); // 1 = açık, 0 = kapalı
        PlayerPrefs.Save();

        if (IsSoundActive)
        {
            // Eğer ses açık ise müziği oynat
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Müzik çalıştı");
            }
        }
        else
        {
            // Ses kapalı ise müziği durdur
            audioSource.Pause();
        }
    }
}


public enum SceneType
{
    Entrance,
    UserInFo,
    Loading,
    Game,
    Settings,
    GameResult,
    InternetCheck,
    Eleven
}
