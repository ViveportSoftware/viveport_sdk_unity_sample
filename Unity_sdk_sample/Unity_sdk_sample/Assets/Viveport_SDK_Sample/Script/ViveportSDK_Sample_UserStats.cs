using System.Collections;
using UnityEngine;
using Viveport;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_UserStats : MonoBehaviour {

    [Serializable]
    public class UnityEventDownloadStatsCallback : UnityEvent<int, string> { }
    public UnityEventDownloadStatsCallback onDownloadStatsComplete;

    [Serializable]
    public class UnityEventUploadStatsCallback : UnityEvent<int, string> { }
    public UnityEventUploadStatsCallback onUploadStatsComplete;

    [Serializable]
    public class UnityEventDownloadLeaderboardCallback : UnityEvent<int, string> { }
    public UnityEventDownloadLeaderboardCallback onDownloadLeaderboardComplete;

    [Serializable]
    public class UnityEventUploadLeaderboardCallback : UnityEvent<int, string> { }
    public UnityEventUploadLeaderboardCallback onUploadLeaderboardComplete;

    private bool _userIsReady = false;
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
            UserStats.IsReady(UserStatsIsReadyCallback);
        }
    }

    private void UserStatsIsReadyCallback(int code)
    {
        if (code == SUCCESS)
        {
            _userIsReady = true;
        }
        else
        {
            Debug.LogError("UserStats IsReady failure ");
        }
    }

    public void DownloadStats()
    {
        if(_userIsReady)
            UserStats.DownloadStats(DownloadStatsHandler);
    }

    public void UploadStats()
    {
        if (_userIsReady)
            UserStats.UploadStats(UploadStatsHandler);
    }

    public void DownloadLeaderboard(string leaderboardName, UserStats.LeaderBoardRequestType requestType, UserStats.LeaderBoardTimeRange timeRange, int rangeStart, int rangeEnd)
    {
        if (_userIsReady)
            UserStats.DownloadLeaderboardScores(DownloadLeaderboardHandler, leaderboardName, requestType, timeRange, rangeStart, rangeEnd);
    }

    public void UploadLeaderboard(string leaderboardName, int score)
    {
        if (_userIsReady)
            UserStats.UploadLeaderboardScore(UploadLeaderboardHandler, leaderboardName, score);
    }

    private void DownloadStatsHandler(int code)
    {
        if (code == 0)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDownloadStatsComplete.Invoke(code, null);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDownloadStatsComplete.Invoke(code, "DownloadStats failure.");
            });
        }
    }

    private void UploadStatsHandler(int code)
    {
        if(code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onUploadStatsComplete.Invoke(code, null);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onUploadStatsComplete.Invoke(code, "UploadStats failure.");
            });
        }
    }

    private void DownloadLeaderboardHandler(int code)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDownloadLeaderboardComplete.Invoke(code, null);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onDownloadLeaderboardComplete.Invoke(code, "DownloadLeaderboard failure.");
            });
        }
    }

    private void UploadLeaderboardHandler(int code)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onUploadLeaderboardComplete.Invoke(code, null);
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onUploadLeaderboardComplete.Invoke(code, "UploadLeaderboard failure.");
            });
        }
    }
}
