using System;
using UnityEngine;
using UnityEngine.Events;
using Viveport;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_SessionToken : MonoBehaviour {

    [Serializable]
    public class UnityEventSessionTokenCallback : UnityEvent<int, string> { }
    public UnityEventSessionTokenCallback onSessionTokenComplete;

    private bool _tokenIsReady = false;
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
            Token.IsReady(IsTokenReadyHandler);
        }
    }

    private void IsTokenReadyHandler(int code)
    {
        if (code != SUCCESS)
        {
            Debug.LogError("Platform setup error, please close the content ...");
            return;
        }
        _tokenIsReady = true;
    }

    public void GetSessionToken()
    {
        if(_tokenIsReady)
            Token.GetSessionToken(GetSessionTokenHandler);
    }

    private void GetSessionTokenHandler(int code, string message)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onSessionTokenComplete.Invoke(code, message);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onSessionTokenComplete.Invoke(code, message);
            });
        }
    }
}
