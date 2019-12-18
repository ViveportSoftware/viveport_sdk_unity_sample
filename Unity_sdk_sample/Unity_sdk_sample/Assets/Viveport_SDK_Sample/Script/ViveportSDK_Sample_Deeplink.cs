using UnityEngine;
using Viveport;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_Deeplink : MonoBehaviour {

    [Serializable]
    public class UnityEventDeeplinkCallback : UnityEvent<int, string> { }
    public UnityEventDeeplinkCallback onDeeplinkComplete;

    private bool deeplinkIsReady = false;
    private const int SUCCESS = 0;

    private void Awake()
    {
        var curTopApi = gameObject.GetComponent<ViveportSDK_Sample_TopApi>();
        curTopApi.onInitComplete.AddListener(InitIsStart);
    }

    private void InitIsStart(int code, string message)
    {
        if (code == SUCCESS)
        {
            Deeplink.IsReady(DeeplinkIsReadyCallback);
        }
    }

    private void DeeplinkIsReadyCallback(int code)
    {
        if (code == SUCCESS)
        {
            deeplinkIsReady = true;
        }
        else
        {
            Debug.LogError("Deeplink IsReady failure ");
        }
    }

    //Currently, launchData cannot be empty.If you don't need to send data to target app, please send " " or "N/A" to avoid exceptions.
    public void GoToApp(string viveportId, string launchData)
    {
        if (deeplinkIsReady)
            Deeplink.GoToApp(GoToAppHandler, viveportId,launchData);
    }

    //Currently, launchData cannot be empty. If you don't need to send data to target app, please send " " or "N/A" to avoid exceptions.
    public void GoToApp(string viveportId, string launchData, string branchName)
    {
        if (deeplinkIsReady)
            Deeplink.GoToApp(GoToAppHandler, viveportId, launchData, branchName);
    }

    public void GoToStore(string viveportId = "")
    {
        if (deeplinkIsReady)
            Deeplink.GoToStore(GoToStore, viveportId);
    }

    //Currently, launchData cannot be empty.If you don't need to send data to target app, please send " " or "N/A" to avoid exceptions.
    public void GoToAppOrGoToStore(string viveportId, string launchData)
    {
        if (deeplinkIsReady)
            Deeplink.GoToAppOrGoToStore(GoToAppOrGoToStoreHandler, viveportId, launchData);
    }

    public string GetLaunchData()
    {
        if (deeplinkIsReady)
            return Deeplink.GetAppLaunchData();
        else
            return "";
    }

    private void GoToAppHandler(int code, string message)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
    }

    private void GoToStore(int code, string message)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
    }

    private void GoToAppOrGoToStoreHandler(int code, string message)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDeeplinkComplete.Invoke(code, message);
            });
        }
    }

}
