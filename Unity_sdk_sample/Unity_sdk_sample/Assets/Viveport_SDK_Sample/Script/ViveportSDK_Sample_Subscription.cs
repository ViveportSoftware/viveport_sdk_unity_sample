using UnityEngine;
using Viveport;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_Subscription : MonoBehaviour {

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
            Subscription.IsReady(IsReadyHandler);
        }
    }

    private void IsReadyHandler(int code, string message)
    {
        if (code == SUCCESS)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                Debug.Log("Subscription IsReady success ");
            });
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(()=> {
                Debug.LogError("Subscription IsReady failure ");
            });
        }
    }
}
