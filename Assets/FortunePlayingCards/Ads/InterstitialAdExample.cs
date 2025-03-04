using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdExample : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "fortune_android";
    [SerializeField] string _iOsAdUnitId = "fortune";
    private string _adUnitId;
    private bool isLoaded = false;  // Reklamın yüklenme durumu

    void Awake()
    {
        // Platforma göre Ad Unit ID'yi belirle
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOsAdUnitId : _androidAdUnitId;
    }

    // Reklam içeriğini Ad Unit'e yükle
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // Yüklenen içeriği göster
    public void ShowAd()
    {
        if (isLoaded)
        {
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            Debug.Log("Ad is not loaded yet.");
        }
    }

    // Reklam başarıyla yüklendiğinde çağrılacak metod
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        isLoaded = true;  // Reklam başarıyla yüklendi
    }

    // Reklam yükleme hatası oluştuğunda çağrılacak metod
    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        isLoaded = false;  // Reklam yüklenemedi
    }

    // Reklam gösterme hatası oluştuğunda çağrılacak metod
    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        isLoaded = false;  // Başarısızlık durumunda yeniden yükleme gerekebilir
    }

    // Diğer gerekli metodlar
    public void OnUnityAdsShowStart(string _adUnitId)
    {
        Debug.Log("Ad Started: " + _adUnitId);
    }

    public void OnUnityAdsShowClick(string _adUnitId)
    {
        Debug.Log("Ad Clicked: " + _adUnitId);
    }

    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Ad Completed: {_adUnitId}, Completion State: {showCompletionState}");
    }

    // Reklamın yüklü olup olmadığını kontrol eden metod
    public bool IsLoaded()
    {
        return isLoaded;
    }
}
