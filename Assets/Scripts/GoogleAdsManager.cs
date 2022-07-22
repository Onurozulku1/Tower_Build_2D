//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;

//public class GoogleAdsManager : MonoBehaviour
//{
//    private int interstitialCounter = 0;

//    private BannerView bannerView;
//    private InterstitialAd interstitialAd;

//    public void Start()
//    {
//        MobileAds.Initialize(initStatus => { });
//        this.RequestBanner();
//        this.RequestInterstitial();
//    }

//    private void RequestBanner()
//    {
//#if UNITY_ANDROID
//        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
//#elif UNITY_IPHONE
//            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

//        AdRequest request = new AdRequest.Builder().Build();
//        this.bannerView.LoadAd(request);
//    }

//    private void RequestInterstitial()
//    {
//#if UNITY_ANDROID
//        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
//#elif UNITY_IPHONE
//            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
//#else
//            string adUnitId = "unexpected_platform";
//#endif

//        this.interstitialAd = new InterstitialAd(adUnitId);

//        AdRequest request = new AdRequest.Builder().Build();
//        this.interstitialAd.LoadAd(request);
//    }

//    private void CheckInterstitial()
//    {
//        interstitialCounter++;
//        if (interstitialCounter >= 4)
//        {
//            this.RequestInterstitial();
//            interstitialAd.Show();
//            interstitialCounter = 0;
//        }
//    }

//    private void OnEnable()
//    {
//        GameController.GameEnding += CheckInterstitial;
//    }

//    private void OnDisable()
//    {
//        GameController.GameEnding -= CheckInterstitial;
//    }


//}
