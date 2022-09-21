using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;

public class GoogleAdsManager : MonoBehaviour
{
    private int interstitialCounter = 0;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;


    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;

    public static GoogleAdsManager instance;
    private void Awake()
    {
        instance = this;
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestAndLoadRewardedAd();

        if (PlayerPrefs.GetInt("removeAds") == 1)
            return;

        RequestBannerAd();
        RequestAndLoadInterstitialAd();
    }

    private void CheckInterstitial()
    {
        if (PlayerPrefs.GetInt("removeAds") == 1)
            return;

        interstitialCounter++;
        if (interstitialCounter >= 3)
        {
            ShowInterstitialAd();
            interstitialCounter = 0;
        }
    }
    

    #region BANNER ADS

    public void RequestBannerAd()
    {
        Debug.Log("Requesting Banner ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) =>
        {
            Debug.Log("Banner ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log("Banner ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        bannerView.OnAdOpening += (sender, args) =>
        {
            Debug.Log("Banner ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        bannerView.OnAdClosed += (sender, args) =>
        {
            Debug.Log("Banner ad closed.");
            OnAdClosedEvent.Invoke();
        };
        bannerView.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            Debug.Log(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        Debug.Log("Requesting Interstitial ad.");

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) =>
        {
            Debug.Log("Interstitial ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log("Interstitial ad failed to load with error: " + args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        interstitialAd.OnAdOpening += (sender, args) =>
        {
            Debug.Log("Interstitial ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        interstitialAd.OnAdClosed += (sender, args) =>
        {
            Debug.Log("Interstitial ad closed.");
            OnAdClosedEvent.Invoke();
        };
        interstitialAd.OnAdDidRecordImpression += (sender, args) =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdFailedToShow += (sender, args) =>
        {
            Debug.Log("Interstitial ad failed to show.");
        };
        interstitialAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Interstitial ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            Debug.Log(msg);
        };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        Debug.Log("Requesting Rewarded ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            Debug.Log("Reward ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            Debug.Log("Reward ad failed to load.");
            OnAdFailedToLoadEvent.Invoke();
        };
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            Debug.Log("Reward ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            Debug.Log("Reward ad failed to show with error: " + args.AdError.GetMessage());
            OnAdFailedToShowEvent.Invoke();
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            Debug.Log("Reward ad closed.");
            OnAdClosedEvent.Invoke();
        };
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            Debug.Log("User earned Reward ad reward: " + args.Amount);
            OnUserEarnedRewardEvent.Invoke();
        };
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            Debug.Log("Reward ad recorded an impression.");
        };
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            Debug.Log(msg);
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAdd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
        }
    }

    #endregion

    private void OnEnable()
    {
        GameController.GameEnding += CheckInterstitial;
    }

    private void OnDisable()
    {
        GameController.GameEnding -= CheckInterstitial;
    }

    
}
