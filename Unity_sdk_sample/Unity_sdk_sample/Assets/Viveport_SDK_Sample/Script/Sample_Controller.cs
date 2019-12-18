using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sample_Controller : MonoBehaviour {

    public Text ConsoleText;
    public GameObject ViveportSDKManager;
    public Button UserStatsBtn, ClearBtn;
    public Button IAPStartBtn, IAPSubscribeBtn;
    public Button DownloadStatsBtn, UploadStatsBtn, DownloadLeadervoardBtn, UploadLeadervoardBtn;
    public Button GetSubscriptionBtn, CheckDLCBtn;
    public Button GoToAppBtn, GoToStoreBtn, GoToAppOrGoToStoreBtn, GetLaunchDataBtn, GoHomeBtn;
    public Button GetSessionTokenBtn;

    private string _sampleStats = "SampleStats";
    private string _sampleAchievements = "SampleAchievements";
    private string _sampleLeadervoard = "SampleLeadervoard";
    private string _goToAppViveportId = "80f7cd51-c1c5-4353-bac5-b2e47223e358";
    private string _goToStoreViveportId = "bbbc73fc-b018-42ce-a049-439ab378dbc6";
    private string _launchData = "Start_Content";
    private string _launchBranchName = "PROD"; // PROD or BETA
    private string _dlcAppId = "";
    private int _dlcIndex = 0;
    private bool _isDLCAvailable = false;

    private ViveportSDK_Sample_IAP _curIAP;
    private ViveportSDK_Sample_UserStats _curUserStats;
    private const int SUCCESS = 0;

    private void Awake()
    {
        var curTopApi = ViveportSDKManager.GetComponent<ViveportSDK_Sample_TopApi>();
        if (curTopApi)
        {
            //Add Viveport TopApi Action
            curTopApi.onInitComplete.AddListener(SDKInitComplete);
        }
        else
        {
            Debug.LogError("ViveportSDK_Sample_TopApi isn't exist");
        }

        var curDRM = ViveportSDKManager.GetComponent<ViveportSDK_Sample_DRM>();
        if (curDRM)
        {
            //Add Viveport DRM Action
            curDRM.onDRMComplete.AddListener(DRMComplete);
        }
        else
        {
            Debug.LogError("ViveportSDK_Sample_DRM isn't exist");
        }


        var curUserProfile = ViveportSDKManager.GetComponent<ViveportSDK_Sample_UserProfile>();
        if (curUserProfile)
        {
            //Add Button Click Event 
            curUserProfile.onUserProfileComplete.AddListener(GetUserStats);
            //Add Viveport UserStats Action
            UserStatsBtn.onClick.AddListener(curUserProfile.StartUserStats);
        }
        else
        {
            Debug.LogError("ViveportSDK_Sample_UserStats isn't exist");
        }

        _curIAP = ViveportSDKManager.GetComponent<ViveportSDK_Sample_IAP>();
        if (_curIAP)
        {
            //Add Button Click Event 
            IAPStartBtn.onClick.AddListener(()=> {
                _curIAP.StartIAPRequest("1");
            });
            IAPSubscribeBtn.onClick.AddListener(()=> {
                _curIAP.StartIAPRequestSubscription("1", "month", 3, "month", 3, 1, "samplePlanId");
            });

            //Add Viveport IAP Action
            _curIAP.onIAPFailure.AddListener(IAPOnFailure);
            _curIAP.onIAPIsReadySuccess.AddListener(IAPIsReadyCallback);
            _curIAP.onIAPRequestSuccess.AddListener(IAPRequestCallback);
            _curIAP.onIAPurchaseSuccess.AddListener(IAPurchaseCallback);
            _curIAP.onIAPQuerySuccess.AddListener(IAPQueryCallback);
            _curIAP.onIAPQueryListSuccess.AddListener(IAPQueryListCallback);

            _curIAP.onIAPRequestSubscriptionSuccess.AddListener(IAPRequestSubscriptionCallback);
            _curIAP.onIAPSubscribeSuccess.AddListener(IAPSubscribeCallback);
            _curIAP.onIAPQuerySubscriptionSuccess.AddListener(IAPQuerySubscriptionCallback);

        }

        _curUserStats = ViveportSDKManager.GetComponent<ViveportSDK_Sample_UserStats>();
        if (_curUserStats)
        {
            //Add Button Click Event 
            DownloadStatsBtn.onClick.AddListener(_curUserStats.DownloadStats);
            UploadStatsBtn.onClick.AddListener(_curUserStats.UploadStats);
            DownloadLeadervoardBtn.onClick.AddListener(()=> {
                _curUserStats.DownloadLeaderboard(_sampleLeadervoard, Viveport.UserStats.LeaderBoardRequestType.GlobalDataAroundUser, Viveport.UserStats.LeaderBoardTimeRange.AllTime, -5, 5);
            });
            UploadLeadervoardBtn.onClick.AddListener(()=> {
                _curUserStats.UploadLeaderboard(_sampleLeadervoard, 999);
            });

            //Add Viveport UserStats Action
            _curUserStats.onDownloadStatsComplete.AddListener(DownloadStatsComplete);
            _curUserStats.onUploadStatsComplete.AddListener(UploadStatsComplete);
            _curUserStats.onDownloadLeaderboardComplete.AddListener(DownloadLeaderboardComplete);
            _curUserStats.onUploadLeaderboardComplete.AddListener(UploadLeaderboardComplete);
        }

        var curSubscription = ViveportSDKManager.GetComponent<ViveportSDK_Sample_Subscription>();
        if (curSubscription)
        {
            //Add Button Click Event 
            GetSubscriptionBtn.onClick.AddListener(GetSubscription);
        }

        var curDLC = ViveportSDKManager.GetComponent<ViveportSDK_Sample_DLC>();
        if (curDLC)
        {
            //Add Button Click Event 
            CheckDLCBtn.onClick.AddListener(CheckDLC);
        }

        var curDeeplink = ViveportSDKManager.GetComponent<ViveportSDK_Sample_Deeplink>();
        if (curDeeplink)
        {
            //Add Button Click Event 
            GoToAppBtn.onClick.AddListener(() =>
            {
                curDeeplink.GoToApp(_goToAppViveportId, _launchData);
            });
            GoToStoreBtn.onClick.AddListener(() => {
                curDeeplink.GoToStore(_goToStoreViveportId);
            });
            GoToAppOrGoToStoreBtn.onClick.AddListener(() =>
            {
                curDeeplink.GoToAppOrGoToStore(_goToAppViveportId, _launchData);
            });
            GetLaunchDataBtn.onClick.AddListener(() =>
            {
                ConsoleText.text += "\n <color=#009900> Deeplink.GetLaunchData: " +  curDeeplink.GetLaunchData() + "</color>";
            });
            GoHomeBtn.onClick.AddListener(() =>{
                curDeeplink.GoToStore();
            });

            curDeeplink.onDeeplinkComplete.AddListener(DeeplinkComplete);
        }

        var curSessionToken = ViveportSDKManager.GetComponent<ViveportSDK_Sample_SessionToken>();
        if (curSessionToken)
        {
            //Add Button Click Event
            GetSessionTokenBtn.onClick.AddListener(curSessionToken.GetSessionToken);

            curSessionToken.onSessionTokenComplete.AddListener(SessionTokenComplete);
        }

        ClearBtn.onClick.AddListener(() => { ConsoleText.text = ""; });
    }

    public void SDKInitComplete(int code, string message)
    {
        Debug.Log("Viveport SDK init Result: " + code + " SDK Init Message :" + message);
        ConsoleText.text = message;
    }

    public void DRMComplete(int code, string message, ViveportSDK_Sample_DRM.DRMInfo drmInfo)
    {
        Debug.Log("Viveport DRM Result: " + code + " SDK DRM Message : " + message);
        if (code == SUCCESS)
        {
            ConsoleText.text += "\n <color=#990000>" + message + "</color>";
        }
        else
        {
            ConsoleText.text += "\n <color=#990000>DRM ErrorCode : " + code + " ErrorMessage : " + message + "</color>";
        }
        

        if (drmInfo != null)
        {
            ConsoleText.text += "\n <color=#009900>issueTime : " + drmInfo.IssueTime + "</color>";
            ConsoleText.text += "\n <color=#009900>expirationTime : " + drmInfo.ExpirationTime + "</color>";
            ConsoleText.text += "\n <color=#009900>latestVersion : " + drmInfo.LatestVersion + "</color>";
            ConsoleText.text += "\n <color=#009900>updateRequired : " + drmInfo.UpdateRequired + "</color>";
        }
    }

    public void GetUserStats(int code, string message, ViveportSDK_Sample_UserProfile.UserInfo userInfo)
    {
        Debug.Log("Viveport UserStats Result: " + code + " UserStats Message : " + message);
        if (code == SUCCESS)
            ConsoleText.text += "\n" + message;
        else
            ConsoleText.text += "\n ErrorCode: " + code + " ErrorMessage: " + message;

        if (userInfo != null)
        {
            ConsoleText.text += "\n <color=#009900>UserId : " + userInfo.UserId + "</color>";
            ConsoleText.text += "\n <color=#009900>UserName : " + userInfo.UserName + "</color>";
            ConsoleText.text += "\n <color=#009900>UserAvatarUrl : " + userInfo.UserAvatarUrl + "</color>";
        }
    }

    public void IAPIsReadyCallback(string currencyName)
    {
        ConsoleText.text += "\n <color=#009900>IAP IsReady Success currencyName : " + currencyName + "</color>";
    }

    public void IAPRequestCallback(string purchaseId)
    {
        ConsoleText.text += "\n <color=#009900>IAP Request Success PurchaseId : " + purchaseId + "</color>";
        _curIAP.StartIAPurchase(purchaseId);
    }

    public void IAPurchaseCallback(string purchaseId)
    {
        ConsoleText.text += "\n <color=#009900>IAP Purchase Success : " + purchaseId + "</color>";
        _curIAP.StartIAPQuery();
    }

    public void IAPQueryCallback(Viveport.IAPurchase.QueryResponse purchase)
    {
        ConsoleText.text += "\n <color=#009900>IAP Query Success : " + purchase.purchase_id + "</color> ";
    }

    public void IAPQueryListCallback(List<Viveport.IAPurchase.QueryResponse2> purchaseList)
    {
        ConsoleText.text += "\n <color=#009900>IAP Query All Success Purchase :  </color> ";
        foreach (var item in purchaseList)
        {
            ConsoleText.text += "\n <color=#009900>purchaseList:" + item.purchase_id + " paid time stamp : " + item.paid_timestamp + "</color>";
        }
    }

    public void IAPRequestSubscriptionCallback(string subscriptionId)
    {
        ConsoleText.text += "\n <color=#009900>IAP Request Subscription Success SubscriptionId : " + subscriptionId + "</color>";
        _curIAP.StartSubscribe(subscriptionId);
    }

    public void IAPSubscribeCallback(string subscriptionId)
    {
        ConsoleText.text += "\n <color=#009900>IAP Subscribe Success SubscriptionId : " + subscriptionId + "</color>";
        _curIAP.StartQuerySubscription();
    }

    public void IAPQuerySubscriptionCallback(Viveport.IAPurchase.Subscription[] subscriptions)
    {
        foreach (var item in subscriptions)
        {
            ConsoleText.text += "\n <color=#009900>IAP QuerySubscription Success SubscriptionId : " + item.subscription_id + "</color>";
        }
    }

    public void IAPOnFailure(int code, string message)
    {
        ConsoleText.text += "\n <color=#990000>ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
    }

    public void DownloadStatsComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            var curStat =  Viveport.UserStats.GetStat(_sampleStats, 0);
            ConsoleText.text += "\n <color=#009900> DownloadStatsComplete SampleStat =  " + curStat + "</color>";
            curStat++;
            Viveport.UserStats.SetStat(_sampleStats, curStat);

            var curAchievementOn = Viveport.UserStats.GetAchievement(_sampleAchievements);
            ConsoleText.text += "\n <color=#009900> DownloadStatsComplete SampleAchievement =  " + curAchievementOn + "</color>";
            Viveport.UserStats.SetAchievement(_sampleAchievements);

        }
        else
        {
            ConsoleText.text += "\n <color=#990000>DownloadStats fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }

    public void UploadStatsComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            var curStat = Viveport.UserStats.GetStat(_sampleStats, 0);
            ConsoleText.text += "\n <color=#009900> UploadStatsComplete SampleStat =  " + curStat + "</color>";

            var curAchievementOn = Viveport.UserStats.GetAchievement(_sampleAchievements);
            ConsoleText.text += "\n <color=#009900> DownloadStatsComplete SampleAchievement =  " + curAchievementOn + "</color>";
        }
        else
        {
            ConsoleText.text += "\n <color=#990000>UploadStats fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }

    public void DownloadLeaderboardComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            var leaderboardScoreCount = Viveport.UserStats.GetLeaderboardScoreCount();
            var leaderboard = Viveport.UserStats.GetLeaderboardScore(0);
            var leaderboardSortMethod = Viveport.UserStats.GetLeaderboardSortMethod();
            var leaderboardDisplayType = Viveport.UserStats.GetLeaderboardDisplayType();
            ConsoleText.text += "\n <color=#009900> DownloadLeaderboardComplete leaderboardScoreCount =  " + leaderboardScoreCount + "</color>";
            ConsoleText.text += string.Format("\n [GetLeaderboard] <color=#009900>Rank: {0}, Score: {1}, UserName: {2}.</color>", leaderboard.Rank, leaderboard.Score, leaderboard.UserName);

        }
        else
        {
            ConsoleText.text += "\n <color=#990000>DownloadLeaderboardComplete fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }

    public void UploadLeaderboardComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            ConsoleText.text += "\n <color=#009900> UploadLeaderboardComplete </color>";
        }
        else
        {
            ConsoleText.text += "\n <color=#990000>UploadLeaderboardComplete fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }

    public void GetSubscription()
    {
        var userStatus = Viveport.Subscription.GetUserStatus();
        var isWindowsSubscriber = userStatus.Platforms.Contains(Viveport.SubscriptionStatus.Platform.Windows) ? "true" : "false";
        var isAndroidSubscriber = userStatus.Platforms.Contains(Viveport.SubscriptionStatus.Platform.Android) ? "true" : "false";
        var transactionType = "";

        switch (userStatus.Type)
        {
            case Viveport.SubscriptionStatus.TransactionType.Unknown:
                transactionType = "Unknown";
                break;
            case Viveport.SubscriptionStatus.TransactionType.Paid:
                transactionType = "Paid";
                break;
            case Viveport.SubscriptionStatus.TransactionType.Redeem:
                transactionType = "Redeem";
                break;
            case Viveport.SubscriptionStatus.TransactionType.FreeTrial:
                transactionType = "FreeTrial";
                break;
            default:
                transactionType = "Unknown";
                break;
        }
        ConsoleText.text += string.Format("\n [GetSubscription] <color=#009900>isAndroidSubscriber: {0}, isWindowsSubscriber: {1}, transactionType: {2}.</color>", isAndroidSubscriber, isWindowsSubscriber, transactionType);
    }

    public void CheckDLC()
    {
        var dlcCount = Viveport.DLC.GetCount();
        ConsoleText.text += string.Format("\n [DLC][GetIsAvailable] <color=#009900>Dlc Count: {0}.</color>", dlcCount);

        var isInRange = Viveport.DLC.GetIsAvailable(_dlcIndex, out _dlcAppId, out _isDLCAvailable);
        if (isInRange)
        {
            ConsoleText.text += string.Format("\n [DLC][GetIsAvailable] <color=#009900>Is DLC available: {0}, DLC Viveport ID: {1}.</color>", _isDLCAvailable, _dlcAppId);
        }
    }

    public void DeeplinkComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            ConsoleText.text += "\n <color=#009900> DeeplinkComplete :" + message　+ "</color>";
        }
        else
        {
            ConsoleText.text += "\n <color=#990000>DeeplinkComplete fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }

    public void SessionTokenComplete(int code, string message)
    {
        if (code == SUCCESS)
        {
            ConsoleText.text += "\n <color=#009900> GetSessionToken :" + message + "</color>";
        }
        else
        {
            ConsoleText.text += "\n <color=#990000>GetSessionTokenComplete fail  ErrorCode: " + code + " ErrorMessage: " + message + "</color>";
        }
    }
}
