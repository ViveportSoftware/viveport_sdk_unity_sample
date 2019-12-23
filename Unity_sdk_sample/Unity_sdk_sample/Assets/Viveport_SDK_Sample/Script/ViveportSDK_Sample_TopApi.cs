using UnityEngine;
using Viveport;
using System;
using UnityEngine.Events;

public class ViveportSDK_Sample_TopApi : MonoBehaviour {

    // Get a VIVEPORT ID and VIVEPORT Key from the VIVEPORT Developer Console. Please refer to here:
    // https://developer.viveport.com/documents/sdk/en/viveport_sdk/definition/get_viveportid.html

    public string VIVEPORT_ID = "";           // replace with developer VIVEPORT ID in Unity editor Inspctor
    public string VIVEPORT_KEY = "";         // replace with developer VIVEPORT Key in Unity editor Inspctor

    public static string viveport_id;
    public static string viveport_key;
    private const int SUCCESS = 0;

    [Serializable]
    public class UnityEventSDKCallback : UnityEvent<int,string> { }

    public UnityEventSDKCallback onInitComplete;

    void Awake()
    {
        if (string.IsNullOrEmpty(VIVEPORT_ID) || string.IsNullOrEmpty(VIVEPORT_KEY))
        {
            Debug.LogError("replace with developer VIVEPORT ID / VIVEPORT Key in ViveportSDK_Sample_TopApi Component ");
            return;
        }
        viveport_id = VIVEPORT_ID;
        viveport_key = VIVEPORT_KEY;

        var mainThreadDispatcher = FindObjectOfType<MainThreadDispatcher>();
        if (!mainThreadDispatcher)
        {
            var main = new GameObject();
            main.AddComponent<MainThreadDispatcher>();
            main.name = "MainThreadDispatcher";
        }
    }

    private void Start()
    {
        Api.Init(InitStatusHandler, VIVEPORT_ID);       // initialize VIVEPORT platform
    }

    void OnDestroy()
    {
        Api.Shutdown(ShutdownHandler);
    }

    private void InitStatusHandler(int code)          // The callback of Api.init()
    {
        if (code == SUCCESS)
        {
            if (onInitComplete != null)
            {
                Api.QueryRuntimeMode(QueryRunTimeHandler);
            }
        }
        else
        {
            Debug.Log("VIVEPORT init fail");
            if (onInitComplete != null)
            {
                onInitComplete.Invoke(code, "<color=#990000>VIVEPORT init fail !!</color>");
            }
            Application.Quit();
            return;                                             // the response of Api.Init() is fail
        }
    }

    private void QueryRunTimeHandler(int code, int nMode)
    {
        if (code == SUCCESS)
        {
            Viveport.Core.Logger.Log("QueryRunTimeHandler is successful" + code + "Running mode is " + nMode);
            // nMode = 1 (Viveport Desktop mode), nMode = 2 (Viveport Arcade mode)
            if (nMode == 1)
            {
                // Use Viveport API
                Debug.Log("VIVEPORT init pass");
                onInitComplete.Invoke(code, " <color=#009900>ViveportSDK Init is Complete !!</color>");
            }
            else
            {
                // Please use Viveport Arcade API , this sample code does not support  Viveport Arcade API
                // Viveport Arcade API : https://developer.viveport.com/documents/sdk/en/api_arcade.html
                Debug.Log("VIVEPORT Arcade init pass");
                onInitComplete.Invoke(2, "<color=#990000>ViveportSDK Arcade Init is Complete !!</color>");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        else
        {
            Debug.Log("QueryRunTimeHandler error: " + code);
        }
    }

    private static void ShutdownHandler(int code)            // The callback of Api.Shutdown()
    {
        if (code == SUCCESS)
        {
            Application.Quit();                                 // the response of Api.Shutdown() is success, close the content
        }
        else
        {
            return;                                             // the response of Api.Shutdown() is fail
        }
    }
}
