using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine.Advertisements;
using System.Collections;
public enum UserConsent
{
    Unset = 0,
    Accept = 1,
    Deny = 2
}
public enum BannerBoxPos // your custom enumeration
{
    bottomleft,
    bottomRight,
    TopLeft, TopRight, Center, Top, Bottom, CenterLeft, CenterRight
};
public class AdmobAds : MonoBehaviour, IUnityAdsListener
{
    public string GameID = "ca-app-pub-9496460709444277~8576776327";
    public string UnityAdId = "3711077";
    string mySurfacingId = "rewardedVideo";
    string mySurfacingIdbanner = "banner";
    // Sample ads
    public string bannerAdId1 = "ca-app-pub-3940256099942544/6300978111";
    public string bannerAdId2 = "ca-app-pub-3940256099942544/6300978111";
    public string bannerAdId3 = "ca-app-pub-3940256099942544/6300978111";
    public string bannerAdId4 = "ca-app-pub-3940256099942544/6300978111";
    public string bannerAdId5 = "ca-app-pub-3940256099942544/6300978111";
    public string InterstitialAdID = "ca-app-pub-3940256099942544/1033173712";
    public string rewarded_Ad_ID = "ca-app-pub-3940256099942544/5224354917";

    public BannerBoxPos boxbannerpos = BannerBoxPos.bottomleft;
    public BannerView bannerAdBottomLeft, bannerAdBottomRight, bannerAdTopLeft, bannerAdTopRight, BannerAdBox;
    public InterstitialAd interstitial;
    public RewardedAd rewardedAd;

    private const string userConsent = "UserConsent";
    private const string ccpaConsent = "CcpaConsent";
    private const string removeAds = "RemoveAds";
    private static bool initialized;
    public static AdmobAds instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        this.rewardedAd = new RewardedAd(rewarded_Ad_ID);

        //rewardedAd = RewardBasedVideoAd.Instance;
        MobileAds.Initialize(initStatus => { });
        Advertisement.Initialize(UnityAdId);
        initialized = true;



    }


    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        //MobileAds.Initialize(GameID);
        // unity banner callling 	//StartCoroutine(ShowBannerWhenInitialized());

    }
    //set user consents and get user consents 

    public void SetUserConsent(bool accept)
    {
        if (accept == true)
        {
            PlayerPrefs.SetInt(userConsent, (int)UserConsent.Accept);
        }
        else
        {
            PlayerPrefs.SetInt(userConsent, (int)UserConsent.Deny);
        }
        if (initialized == true)
        {
            UpdateUserConsent();
        }
    }
    public void SetCCPAConsent(bool accept)
    {
        if (accept == true)
        {
            PlayerPrefs.SetInt(ccpaConsent, (int)UserConsent.Accept);
        }
        else
        {
            PlayerPrefs.SetInt(ccpaConsent, (int)UserConsent.Deny);
        }
        if (initialized == true)
        {
            UpdateUserConsent();
        }
    }

    public bool CanShowAds()
    {
        if (!PlayerPrefs.HasKey(removeAds))
        {
            return true;
        }
        else
        {
            if (PlayerPrefs.GetInt(removeAds) == 0)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveAds(bool remove)
    {
        if (remove == true)
        {
            PlayerPrefs.SetInt(removeAds, 1);
            //if banner is active and user bought remove ads the banner will automatically hide
            hideBannerBottomLeft();
            hideBannerBottomRight();
            hideBannerTopLeft();
            hideBannerTopRight();
        }
        else
        {
            PlayerPrefs.SetInt(removeAds, 0);
        }
    }
    private void UpdateUserConsent()
    {
        UpdateConsent(GetConsent(userConsent), GetConsent(ccpaConsent));
    }
    public void UpdateConsent(UserConsent consent, UserConsent ccpaConsent)
    {

    }
    private UserConsent GetConsent(string fileName)
    {
        if (!ConsentWasSet(fileName))
            return UserConsent.Unset;
        return (UserConsent)PlayerPrefs.GetInt(fileName);
    }

    private bool ConsentWasSet(string fileName)
    {
        return PlayerPrefs.HasKey(fileName);
    }
    public UserConsent GetUserConsent()
    {
        return GetConsent(userConsent);
    }
    public bool UserConsentWasSet()
    {
        return PlayerPrefs.HasKey(userConsent);
    }

    public bool CCPAConsentWasSet()
    {
        return PlayerPrefs.HasKey(ccpaConsent);
    }
    //end consents




    #region rewarded Video Ads

    public void loadRewardVideo()
    {

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        //rewardedAd.LoadAd(new AdRequest.Builder().Build(), rewarded_Ad_ID);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        //rewardedAd.on += HandleOnRewardAdleavingApp;

    }
    public IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (CanShowAds())
        {
            Advertisement.Banner.Show(mySurfacingIdbanner);
        }
        Advertisement.Banner.SetPosition(BannerPosition.CENTER);
    }
    public void showUnityInterstitialAd()
    {
        if (CanShowAds())
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
            else
            {
                Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
            }
        }


    }
    // unity rewarded ADs 

    public void ShowUnityRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("i got the reward from unity");
            //	admanager.instance.DebuggingText.text = "i got the reward " + PlayerPrefs.GetInt("RewardKey");
            admanager.instance.rewardOfRewardedVideo();



        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        //Advertisement.RemoveListener(this);
    }


    //unity ads finish 
    /// rewarded video events //////////////////////////////////////////////

    public event EventHandler<EventArgs> OnAdLoaded;

    public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

    public event EventHandler<EventArgs> OnAdOpening;

    public event EventHandler<EventArgs> OnAdStarted;

    public event EventHandler<EventArgs> OnAdClosed;

    public event EventHandler<Reward> OnAdRewarded;

    public event EventHandler<EventArgs> OnAdLeavingApplication;

    public event EventHandler<EventArgs> OnAdCompleted;

    /// Rewared events //////////////////////////



    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Video Loaded");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Video not loaded");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("Video Loading");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("Video Loading failed");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Video Loading failed");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        /// reward the player here --------------------
        // GameManager.GMinstance.rewaredPlayer();
        Debug.Log("i got the reward from admob");
        //admanager.instance.DebuggingText.text = "i got the reward " + PlayerPrefs.GetInt("RewardKey");
        admanager.instance.rewardOfRewardedVideo();

    }

    public void HandleOnRewardAdleavingApp(object sender, EventArgs args)
    {
        Debug.Log("when user clicks the video and open a new window");
    }



    public void showAdmobRewardedVideoAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            loadRewardVideo();
            Debug.Log("Rewarded Video ad not loaded");
        }
    }

    #endregion

    #region banner

    public void reqBannerAdBottomLeft()
    {
        if (CanShowAds())
        {
            if (bannerAdBottomLeft == null)
            {

                bannerAdBottomLeft = new BannerView(bannerAdId1, AdSize.Banner, AdPosition.BottomLeft);

                // Called when an ad request has successfully loaded.
                bannerAdBottomLeft.OnAdLoaded += this.HandleOnAdLoaded;
                // Called when an ad request failed to load.
                bannerAdBottomLeft.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdBottomLeft.LoadAd(request);
            }
            else
            {

                hideBannerBottomLeft();
                reqBannerAdBottomLeft();
            }
        }
    }
    public void hideBannerBottomLeft()
    {
        if (bannerAdBottomLeft != null)
        {
            //bannerAdBottomLeft.Destroy();
            bannerAdBottomLeft.Hide();
            bannerAdBottomLeft = null;
        }
        //}
    }
    public void reqBannerAdBottomRight()
    {
        if (CanShowAds())
        {
            if (bannerAdBottomRight == null)
            {
                bannerAdBottomRight = new BannerView(bannerAdId2, AdSize.Banner, AdPosition.BottomRight);

                // Called when an ad request has successfully loaded.
                bannerAdBottomRight.OnAdLoaded += this.HandleOnAdLoaded;
                // Called when an ad request failed to load.
                bannerAdBottomRight.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdBottomRight.LoadAd(request);
            }
            else
            {
                hideBannerBottomRight();
                reqBannerAdBottomRight();
            }
        }
    }
    public void hideBannerBottomRight()
    {
        if (bannerAdBottomRight != null)
        {
            //bannerAdBottomRight.Destroy();
            bannerAdBottomRight.Hide();
            bannerAdBottomRight = null;
        }
    }
    public void reqBannerAdTopLeft()
    {
        if (CanShowAds())
        {
            if (bannerAdTopLeft == null)
            {
                bannerAdTopLeft = new BannerView(bannerAdId3, AdSize.Banner, AdPosition.TopLeft);

                // Called when an ad request has successfully loaded.
                bannerAdTopLeft.OnAdLoaded += this.HandleOnAdLoaded;
                // Called when an ad request failed to load.
                bannerAdTopLeft.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                bannerAdTopLeft.LoadAd(request);
            }
            else
            {
                hideBannerTopLeft();
                reqBannerAdTopLeft();
            }
        }
    }
    public void hideBannerTopLeft()
    {
        if (bannerAdTopLeft != null)
        {
            //bannerAdTopLeft.Destroy();
            bannerAdTopLeft.Hide();
            bannerAdTopLeft = null;
        }
    }
    public void reqBannerAdTopRight()
    {
        if (CanShowAds())
        {
            if (bannerAdTopRight == null)
            {
                this.bannerAdTopRight = new BannerView(bannerAdId4, AdSize.Banner, AdPosition.TopRight);

                // Called when an ad request has successfully loaded.
                this.bannerAdTopRight.OnAdLoaded += this.HandleOnAdLoaded;
                // Called when an ad request failed to load.
                this.bannerAdTopRight.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                this.bannerAdTopRight.LoadAd(request);
            }
            else
            {
                hideBannerTopRight();
                reqBannerAdTopRight();
            }
        }
    }
    public void hideBannerTopRight()
    {
        if (bannerAdTopRight != null)
        {

            bannerAdTopRight.Hide();
            bannerAdTopRight = null;

        }
    }

    public void reqBannerAdBox()
    {
        if (CanShowAds())
        {
            if (BannerAdBox == null)
            {
                if (boxbannerpos == BannerBoxPos.bottomleft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomLeft);
                else if (boxbannerpos == BannerBoxPos.bottomRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomRight);
                else if (boxbannerpos == BannerBoxPos.TopLeft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.TopLeft);
                else if (boxbannerpos == BannerBoxPos.TopRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.TopRight);
                else if (boxbannerpos == BannerBoxPos.Center)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Center);
                else if (boxbannerpos == BannerBoxPos.Top)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Top);
                else if (boxbannerpos == BannerBoxPos.Bottom)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.Bottom);
                else if (boxbannerpos == BannerBoxPos.CenterLeft)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomLeft);
                else if (boxbannerpos == BannerBoxPos.CenterRight)
                    BannerAdBox = new BannerView(bannerAdId5, AdSize.MediumRectangle, AdPosition.BottomRight);
                // Called when an ad request has successfully loaded.
                BannerAdBox.OnAdLoaded += this.HandleOnAdLoaded;
                // Called when an ad request failed to load.
                BannerAdBox.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

                AdRequest request = new AdRequest.Builder().Build();

                BannerAdBox.LoadAd(request);
            }
            else
            {

                hidereqBannerAdBox();
                reqBannerAdBox();
            }
        }
    }
    public void hidereqBannerAdBox()
    {
        if (BannerAdBox != null)
        {

            BannerAdBox.Hide();
            BannerAdBox = null;
        }

    }


    #endregion

    #region interstitial

    public void requestInterstital()
    {

        this.interstitial = new InterstitialAd(InterstitialAdID);

        this.interstitial.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.interstitial.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.interstitial.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
       // this.interstitial.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();

        this.interstitial.LoadAd(request);

    }

    public void ShowAdmobInterstitialAd()
    {
        if (CanShowAds())
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
        }
    }

    #endregion

    #region adDelegates

    //Delegates that i dont know
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad Loaded");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Interstitial failed to load: ");
        // Handle the ad failed to load event.
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("Ad Closed");
        requestInterstital(); // Optional : in case you want to load another interstial ad rightaway
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

}
