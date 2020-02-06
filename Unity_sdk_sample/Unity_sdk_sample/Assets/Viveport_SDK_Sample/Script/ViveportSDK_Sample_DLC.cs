using System.Collections.Generic;
using UnityEngine;
using Viveport;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_DLC : MonoBehaviour {

    private const int SUCCESS = 0;
    private Dictionary<string, bool> dlcList;

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
            UpdateDLCList();
        }
        else
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                Debug.LogError("DLC IsDlcReady failure ");
            });
        }
    }

    public void UpdateDLCList()
    {
        var dlcCount = DLC.GetCount();
        dlcList = new Dictionary<string, bool>();
        for (int i = 0; i < dlcCount; i++)
        {
            var dlcAppId = "";
            var isDLCAvailable = false;
            DLC.GetIsAvailable(i, out dlcAppId, out isDLCAvailable);
            dlcList.Add(dlcAppId, isDLCAvailable);
        }
    }

    public bool CheckDLCVIVEPORTID(string viveportId)
    {
        if (dlcList == null)
            return false;
        if (dlcList.ContainsKey(viveportId))
            return true;
        else
            return false;
    }
}
