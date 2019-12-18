using System.Collections;
using UnityEngine;
using Viveport;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_UserProfile : MonoBehaviour {

    [Serializable]
    public class UnityEventUserCallback : UnityEvent<int, string, UserInfo> { }

    public UnityEventUserCallback onUserProfileComplete;

    private static UnityEventUserCallback onUserProfileComplete_s; 

    private bool _initIsComplete = false;
    private int SUCCESS = 0;

    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatarUrl { get; set; }
    }

    private void Awake()
    {
        var curTopApi = gameObject.GetComponent<ViveportSDK_Sample_TopApi>();
        curTopApi.onInitComplete.AddListener(InitIsStart);
    }

    private void InitIsStart(int code, string message)
    {
        if (code == SUCCESS)
        {
            onUserProfileComplete_s = onUserProfileComplete;
            _initIsComplete = true;
        }
    }

    /// <summary>
    /// Use Button onClick to Start UserStats
    /// </summary>
    public void StartUserStats()
    {
        if (_initIsComplete)
        {
            User.IsReady(IsReadyHandler);
        }
    }

    private void IsReadyHandler(int code)
    {
        if (code != SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(FailAction());
            return;
            // Handle error
        }

        MainThreadDispatcher.Instance().Enqueue(SuccessAction());
        // Do more things

    }

    private IEnumerator SuccessAction()
    {
        if (onUserProfileComplete_s != null)
        {
            UserInfo curUserInfo = new UserInfo()
            {
                    UserName = User.GetUserName(),
                    UserId = User.GetUserId(),
                    UserAvatarUrl = User.GetUserAvatarUrl()
            };
            onUserProfileComplete_s.Invoke(0, "<color=#009900>Viveport UserStats is Ready !!</color>", curUserInfo);
        }
        yield return null;
    }

    private IEnumerator FailAction()
    {
        if (onUserProfileComplete_s != null)
        {
            onUserProfileComplete_s.Invoke(1, "<color=#009900>Viveport UserStats is fail !!</color>", null);
        }
        yield return null;
    }
}
