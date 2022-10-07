using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppPurchase : MonoBehaviour
{
    private GoogleAdsManager gam;

    private void Awake()
    {
        gam = GoogleAdsManager.instance;
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("removeAds", 1);
        gam.DestroyBannerAd();
    }

    public void BuyToken(int amount)
    {
        MarketManager.token += amount;
        PlayerPrefs.SetInt("token", MarketManager.token);
        UIManager.instance.inGameTokenText.text = MarketManager.token.ToString();
    }
}
