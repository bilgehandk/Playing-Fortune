using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "PlayingFortuneReward_Android";
    [SerializeField] string _iOSAdUnitId = "PlayingFortuneReward_iOS";
    private string _adUnitId;
    private bool _isAdLoaded; // Reklamın yüklenip yüklenmediğini kontrol eder

    void Awake()
    {
        // Platforma göre reklam ID'sini al
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        
    }
    

    // Reklam yüklemek için çağırılan metod
    public void LoadAd()
    {
        Debug.Log("Rewarded Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Reklam başarılı bir şekilde yüklendiğinde çağrılır
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Rewarded Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            _isAdLoaded = true;  // Reklam başarıyla yüklendi
        }
    }

    // Reklamı göstermek için butona basıldığında çağrılır
    public void ShowAd()
    {
        if (_isAdLoaded)
        {
            Advertisement.Show(_adUnitId, this); // Reklam gösterilir
        }
        else
        {
            Debug.Log("Rewarded Ad is not loaded yet.");
            LoadAd(); // Reklam yüklü değilse tekrar yüklemeye çalış
        }
    }

    // Reklam başarılı bir şekilde gösterildiğinde ve tamamlandığında çağrılır
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded Ad Completed, granting reward...");
            GameManager.Instance.isAdRewardedOpened = true;  // Ödülü kazanma
            GameManager.Instance.LoadScene(SceneType.Loading);
        }
    }

    // Reklam yüklenemezse bu metod çağrılır
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _isAdLoaded = false; // Reklam yüklenemedi, tekrar yükleme gerekebilir
    }

    // Reklam gösterim hatası oluşursa bu metod çağrılır
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _isAdLoaded = false; // Reklam gösterim hatası, tekrar yüklenebilir
        LoadAd(); // Tekrar yükle
    }
    
    public bool IsLoaded()
    {
        return _isAdLoaded;
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
}
