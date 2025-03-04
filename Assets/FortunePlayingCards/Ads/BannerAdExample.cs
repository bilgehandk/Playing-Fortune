using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdExample : MonoBehaviour
{
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null; // Platforma göre belirlenecek.
    private bool isLoaded = false; // Reklam yüklendi mi durumu

    void Start()
    {
        // Platforma göre Ad Unit ID al
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Banner pozisyonunu belirle
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    // Banner'ı yüklemek için metod
    public void LoadBanner()
    {
        // Yükleme olaylarını dinleyen opsiyonları ayarla
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Banner reklamını yükle
        Advertisement.Banner.Load(_adUnitId, options);
    }

    // Banner yüklendiğinde çağrılacak metod
    public void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        isLoaded = true; // Banner başarıyla yüklendi
    }

    // Yüklenme sırasında hata olduğunda çağrılacak metod
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        isLoaded = false; // Banner yüklenemedi
    }

    // Banner'ı göstermek için metod
    public void ShowBannerAd()
    {
        if (isLoaded)
        {
            // Banner gösterim olaylarını dinleyen opsiyonları ayarla
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Banner reklamını göster
            Advertisement.Banner.Show(_adUnitId, options);
        }
        else
        {
            Debug.Log("Banner henüz yüklenmedi.");
        }
    }

    // Banner'ı gizlemek için metod
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Banner olayları için callback metodları
    void OnBannerClicked() { Debug.Log("Banner clicked"); }
    void OnBannerShown() { Debug.Log("Banner shown"); }
    void OnBannerHidden() { Debug.Log("Banner hidden"); }

    // Reklamın yüklü olup olmadığını kontrol eden metod
    public bool IsLoaded()
    {
        return isLoaded;
    }
}
