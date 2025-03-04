using System.Net.Mime;
using FortunePlayingCards.Ads;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public AdsInitializer _AdsInitializer;
    public BannerAdExample _BannerAdExample;
    public InterstitialAdExample _InterstitialAd;
    public RewardedAdsButton _RewardedAds;

    public static AdsManager Instance { get; private set; }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (Application.internetReachability!=NetworkReachability.NotReachable)
        {
            _AdsInitializer.InitializeAds();
        
            _BannerAdExample.LoadBanner();
            _InterstitialAd.LoadAd();
            _RewardedAds.LoadAd();
        }
    }

    public void InitializeLoadAds()
    {
        _AdsInitializer.InitializeAds();
        
        _BannerAdExample.LoadBanner();
        _InterstitialAd.LoadAd();
        _RewardedAds.LoadAd();
    }

    // Reklamın yüklenip yüklenmediğini kontrol eden metodlar
    public bool IsBannerAdReady()
    {
        return _BannerAdExample.IsLoaded();  // IsLoaded methodunu BannerAdExample içinde eklemelisin
    }

    public bool IsInterstitialAdReady()
    {
        return _InterstitialAd.IsLoaded();  // IsLoaded methodunu InterstitialAdExample içinde eklemelisin
    }

    public bool IsRewardedAdReady()
    {
        return _RewardedAds.IsLoaded();  // IsLoaded methodunu RewardedAdsButton içinde eklemelisin
    }
}