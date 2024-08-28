using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    private string _bannerAdUnit = "ca-app-pub-4258377019038758/5862103986";
    BannerView _bannerView;

    private string _interAdUnit = "ca-app-pub-4258377019038758/9697407217";
    InterstitialAd _interstitialAd;

    private string _rewardAdUnit = "ca-app-pub-4258377019038758/1430902364";
    RewardedAd _rewardedAd;

    float delayLoadBanner = 1;
    float delayLoadInter = 1;
    float delayLoadReward = 1;

    int interTime = 100;
    [SerializeField] float interTimer = 0;

	int rewardShowTime = 30;
	public float rewardShowTimer = 0;

	bool isShowing = false;

    Action<bool> InterCallBack;
    Action<bool> RewardCallBack;
    // Start is called before the first frame update
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            InitBanner();
            LoadInter();
            LoadRewardedAd();
        });

        interTimer = 5;
        rewardShowTimer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (interTimer >= 0)
            interTimer -= Time.deltaTime;

		if (rewardShowTimer >= 0)
			rewardShowTimer -= Time.deltaTime;
	}

    public bool CheckRewardAd()
    {
        return _rewardedAd.CanShowAd();
    }

    #region Banner
    void InitBanner()
    {
        if (_bannerView == null)
        {
            // Create a 320x50 banner views at coordinate (0,50) on screen.
            _bannerView = new BannerView(_bannerAdUnit, AdSize.Banner, AdPosition.Bottom);
        }

        // create our request used to load the ad.
        

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        

        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
            delayLoadBanner = 1;
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);

            Invoke("LoadBanner",delayLoadBanner);
            delayLoadBanner *= 2;
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
            isShowing = true;
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };

        LoadBanner();
    }

    void LoadBanner()
    {
        var adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }

    public void ShowBanner()
    {
        if(_bannerView == null)
        {
            return;
        }
        
        _bannerView.Show();
    }

    public void HideBanner()
    {
        if (_bannerView == null)
        {
            return;
        }

        _bannerView.Hide();
    }
    #endregion

    #region Inter

    void LoadInter()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        var adRequest = new AdRequest();

        InterstitialAd.Load(_interAdUnit, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  Invoke("LoadInter", delayLoadInter);
                  delayLoadInter *= 2;

                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

              _interstitialAd = ad;
              delayLoadInter = 1;

              _interstitialAd.OnAdPaid += (AdValue adValue) =>
              {
                  Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                      adValue.Value,
                      adValue.CurrencyCode));
              };
              // Raised when an impression is recorded for an ad.
              _interstitialAd.OnAdImpressionRecorded += () =>
              {
                  Debug.Log("Interstitial ad recorded an impression.");
              };
              // Raised when a click is recorded for an ad.
              _interstitialAd.OnAdClicked += () =>
              {
                  Debug.Log("Interstitial ad was clicked.");
              };
              // Raised when an ad opened full screen content.
              _interstitialAd.OnAdFullScreenContentOpened += () =>
              {
                  Debug.Log("Interstitial ad full screen content opened.");
                  isShowing = true;
              };
              // Raised when the ad closed full screen content.
              _interstitialAd.OnAdFullScreenContentClosed += () =>
              {
                  Debug.Log("Interstitial ad full screen content closed.");
                  isShowing = false;
                  LoadInter();
                  this.InterCallBack?.Invoke(true);
              };
              // Raised when the ad failed to open full screen content.
              _interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
              {
                  Debug.LogError("Interstitial ad failed to open full screen content " +
                                 "with error : " + error);
                  LoadInter();
                  delayLoadInter *= 2;
                  this.InterCallBack?.Invoke(false);
              };
          });
    }

    public void ShowInter(Action<bool> callback = null)
    {
        if (_interstitialAd == null)
        {
            callback?.Invoke(false);
            return;
        }

        this.InterCallBack = callback;
        if (interTimer < 0)
        {
            _interstitialAd.Show();
            interTimer = interTime;
        }

    }

    #endregion

    #region Reward
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_rewardAdUnit, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    Invoke("LoadRewardedAd", delayLoadReward);
                    delayLoadReward *= 2;
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                delayLoadReward = 1;
                // Raised when the ad is estimated to have earned money.
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                        adValue.Value,
                        adValue.CurrencyCode));
                };
                // Raised when an impression is recorded for an ad.
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Rewarded ad recorded an impression.");
                };
                // Raised when a click is recorded for an ad.
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Rewarded ad was clicked.");
                };
                // Raised when an ad opened full screen content.
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded ad full screen content opened.");
                    isShowing = true;
                };
                // Raised when the ad closed full screen content.
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Rewarded ad full screen content closed.");
                    isShowing = false;
                    RewardCallBack?.Invoke(false);
                };
                // Raised when the ad failed to open full screen content.
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.LogError("Rewarded ad failed to open full screen content " +
                                   "with error : " + error);
                    Invoke("LoadRewardedAd", delayLoadReward);
                    delayLoadReward *= 2;
                };
            });


    }

    public void ShowRewardedAd(Action<bool> callback = null)
    {
        if (_rewardedAd == null)
        {
            callback?.Invoke(false);
            return;
        }

        if (!_rewardedAd.CanShowAd())
        {
            callback?.Invoke(false);
            return;
        }
        RewardCallBack = callback;
        _rewardedAd.Show(Reward =>
        {
            callback?.Invoke(true);
            RewardCallBack = null;
        });
    }

    public void ResetRewardShowTimer()
    {
        rewardShowTimer = rewardShowTime;
    }
    #endregion
}
