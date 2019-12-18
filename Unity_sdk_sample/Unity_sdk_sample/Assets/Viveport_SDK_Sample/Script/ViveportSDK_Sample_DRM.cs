using System.Collections;
using UnityEngine;
using Viveport;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_DRM : MonoBehaviour {

    [Serializable]
    public class UnityEventDRMCallback : UnityEvent<int, string, DRMInfo> { }

    public UnityEventDRMCallback onDRMComplete;

    private static UnityEventDRMCallback onDRMComplete_s;
    private const int SUCCESS = 0;

    public class DRMInfo
    {
        public long IssueTime { get; set; }
        public long ExpirationTime { get; set; }
        public int LatestVersion { get; set; }
        public bool UpdateRequired { get; set; }

    }

    private void Awake()
    {
        ViveportSDK_Sample_TopApi curTopApi = gameObject.GetComponent<ViveportSDK_Sample_TopApi>();
        curTopApi.onInitComplete.AddListener(InitIsStart);
    }

    /// <summary>
    /// wait ViveportSDK_Sample_TopApi onInitComplete 
    /// </summary>
    /// <param name="inResult">onInitComplete result 0 = success </param>
    /// <param name="message">onInitComplete message </param>
    public void InitIsStart(int code, string message)
    {
        if (code == SUCCESS)
        {
            onDRMComplete_s = onDRMComplete;
            Api.GetLicense(new MyLicenseChecker(), ViveportSDK_Sample_TopApi.viveport_id, ViveportSDK_Sample_TopApi.viveport_key);
        }
    }

    class MyLicenseChecker : Api.LicenseChecker
    {
        DRMInfo curInfo;
        int curErrorCode;
        string curErrorMessage;

        public override void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
        {
            // the response of Api.GetLicense() is DRM success, user is allowed to use the content and continue with content flow
            Debug.Log("Viveport DRM pass");
            Debug.Log("issueTime: " + issueTime);
            Debug.Log("expirationTime: " + expirationTime);
            curInfo = new DRMInfo()
            {
                IssueTime = issueTime,
                ExpirationTime = expirationTime,
                LatestVersion = latestVersion,
                UpdateRequired = updateRequired
            };
            MainThreadDispatcher.Instance().Enqueue(SuccessAction());
        }

        public override void OnFailure(int errorCode, string errorMessage)
        {
            // the response of Api.GetLicense() is DRM fail, user is not allowed to use the content
            Debug.LogError("Viveport DRM fail:" + errorCode + " Message :" + errorMessage);
            curErrorCode = errorCode;
            curErrorMessage = errorMessage;
            MainThreadDispatcher.Instance().Enqueue(FailAction());
        }

        // Use these methods to call Unity functions from the API callbacks on the main thread
        IEnumerator SuccessAction()
        {
            if (onDRMComplete_s != null)
            {
                onDRMComplete_s.Invoke( 0, "<color=#009900>Viveport DRM pass</color>",curInfo);
            }
            yield return null;
        }

        IEnumerator FailAction()
        {
            if (onDRMComplete_s != null)
            {
                onDRMComplete_s.Invoke(curErrorCode, "<color=#990000>" + curErrorMessage + "</color>", null);
            }
            yield return null;
        }
    }

}
