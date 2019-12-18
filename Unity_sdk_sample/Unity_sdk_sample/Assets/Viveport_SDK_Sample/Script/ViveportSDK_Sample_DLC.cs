using UnityEngine;
using Viveport;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_DLC : MonoBehaviour {

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
            DLC.IsDlcReady(IsReadyHandler);
        }
    }

    private void IsReadyHandler(int code)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                Debug.Log("DLC IsDlcReady success ");
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                Debug.LogError("DLC IsDlcReady failure ");
            });
        }
    }
}
