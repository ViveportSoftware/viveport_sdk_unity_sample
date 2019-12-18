using UnityEngine;
using Viveport;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ViveportSDK_Sample_TopApi))]
public class ViveportSDK_Sample_IAP : MonoBehaviour {

    public string VIVEPORT_API_KEY = "";         // replace with developer VIVEPORT API Key in Unity editor Inspctor

    [Serializable]
    public class UnityEventIAPIsRadySuccessCallback : UnityEvent<string> { };
    public UnityEventIAPIsRadySuccessCallback onIAPIsReadySuccess;

    [Serializable]
    public class UnityEventIAPRequestSuccessCallback : UnityEvent<string> { };
    public UnityEventIAPRequestSuccessCallback onIAPRequestSuccess;

    [Serializable]
    public class UnityEventIAPurchaseSuccessCallback : UnityEvent<string> { };
    public UnityEventIAPurchaseSuccessCallback onIAPurchaseSuccess;

    [Serializable]
    public class UnityEventIAPQuerySuccessCallback : UnityEvent<IAPurchase.QueryResponse> { };
    public UnityEventIAPQuerySuccessCallback onIAPQuerySuccess;

    [Serializable]
    public class UnityEventIAPQueryListSuccessCallback : UnityEvent<List<IAPurchase.QueryResponse2>> { };
    public UnityEventIAPQueryListSuccessCallback onIAPQueryListSuccess;

    [Serializable]
    public class UnityEventIAPBalanceSuccessCallback : UnityEvent<string> { };
    public UnityEventIAPBalanceSuccessCallback onIAPBalanceSuccess;

    [Serializable]
    public class UnityEventIAPRequestSubscriptionSuccessCallback : UnityEvent<string> { };
    public UnityEventIAPRequestSubscriptionSuccessCallback onIAPRequestSubscriptionSuccess;

    [Serializable]
    public class UnityEventIAPSubscribeSuccessCallback : UnityEvent<string> { };
    public UnityEventIAPSubscribeSuccessCallback onIAPSubscribeSuccess;

    [Serializable]
    public class UnityEventIAPQuerySubscriptionSuccessCallback : UnityEvent<IAPurchase.Subscription[]> { };
    public UnityEventIAPQuerySubscriptionSuccessCallback onIAPQuerySubscriptionSuccess;

    [Serializable]
    public class UnityEventIAPCancelSubscriptionSuccessCallback : UnityEvent<bool> { };
    public UnityEventIAPCancelSubscriptionSuccessCallback onIAPCancelSubscriptionSuccess;

    [Serializable]
    public class UnityEventIAPFailureCallback : UnityEvent<int, String> { };
    public UnityEventIAPFailureCallback onIAPFailure;

    private static UnityEventIAPIsRadySuccessCallback onIAPIsReadySuccess_s;
    private static UnityEventIAPRequestSuccessCallback onIAPRequestSuccess_s;
    private static UnityEventIAPurchaseSuccessCallback onIAPurchaseSuccess_s;
    private static UnityEventIAPQuerySuccessCallback onIAPQuerySuccess_s;
    private static UnityEventIAPQueryListSuccessCallback onIAPQueryListSuccess_s;
    private static UnityEventIAPBalanceSuccessCallback onIAPBalanceSuccess_s;
    private static UnityEventIAPRequestSubscriptionSuccessCallback onIAPRequestSubscriptionSuccess_s;
    private static UnityEventIAPSubscribeSuccessCallback onIAPSubscribeSuccess_s;
    private static UnityEventIAPQuerySubscriptionSuccessCallback onIAPQuerySubscriptionSuccess_s;
    private static UnityEventIAPCancelSubscriptionSuccessCallback onIAPCancelSubscriptionSuccess_s;
    private static UnityEventIAPFailureCallback onIAPFailure_s;
    private bool _initIsComplete = false;
    private static bool _iapIsReady = false;
    private SampleIAPListener _iAPResult;
    private const int SUCCESS = 0;

    private void Awake()
    {
        ViveportSDK_Sample_TopApi curTopApi = gameObject.GetComponent<ViveportSDK_Sample_TopApi>();
        curTopApi.onInitComplete.AddListener(InitIsStart);
    }

    private void InitIsStart(int code, string message)
    {
        if (code == SUCCESS)
        {
            onIAPIsReadySuccess_s = onIAPIsReadySuccess;
            onIAPRequestSuccess_s = onIAPRequestSuccess;
            onIAPurchaseSuccess_s = onIAPurchaseSuccess;
            onIAPQuerySuccess_s = onIAPQuerySuccess;
            onIAPQueryListSuccess_s = onIAPQueryListSuccess;
            onIAPBalanceSuccess_s = onIAPBalanceSuccess;
            onIAPRequestSubscriptionSuccess_s = onIAPRequestSubscriptionSuccess;
            onIAPSubscribeSuccess_s = onIAPSubscribeSuccess;
            onIAPQuerySubscriptionSuccess_s = onIAPQuerySubscriptionSuccess;
            onIAPCancelSubscriptionSuccess_s = onIAPCancelSubscriptionSuccess;
            onIAPFailure_s = onIAPFailure;
            _iAPResult = new SampleIAPListener();
            _initIsComplete = true;
            IAPurchase.IsReady(_iAPResult, VIVEPORT_API_KEY);
        }
    }

    public void StartIAPRequest(string price)
    {
        if (_iapIsReady)
        {
            IAPurchase.Request(_iAPResult, price);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartIAPurchase(string purchaseId)
    {
        if (_iapIsReady)
        {
            IAPurchase.Purchase(_iAPResult, purchaseId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartIAPQuery(string purchaseId)
    {
        if (_iapIsReady)
        {
            IAPurchase.Query(_iAPResult, purchaseId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartIAPQuery()
    {
        if (_iapIsReady)
        {
            IAPurchase.Query(_iAPResult);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void GetBalance()
    {
        if (_iapIsReady)
        {
            IAPurchase.GetBalance(_iAPResult);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartIAPRequestSubscription(string price, string freeTrialType, int freeTrialValue, string chargePeriodType, int chargePeriodValue, int numberOfChargePeriod, string planId)
    {
        if (_iapIsReady)
        {
            IAPurchase.RequestSubscription(_iAPResult, price, freeTrialType, freeTrialValue, chargePeriodType, chargePeriodValue, numberOfChargePeriod, planId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartIAPRequestSubscriptionWithPlanID(string planId)
    {
        if (_iapIsReady)
        {
            IAPurchase.RequestSubscriptionWithPlanID(_iAPResult, planId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartSubscribe(string subscriptionId)
    {
        if (_iapIsReady)
        {
            IAPurchase.Subscribe(_iAPResult, subscriptionId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartQuerySubscription(string subscriptionId)
    {
        if (_iapIsReady)
        {
            IAPurchase.QuerySubscription(_iAPResult, subscriptionId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void StartQuerySubscription()
    {
        if (_iapIsReady)
        {
            IAPurchase.QuerySubscription(_iAPResult, null);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    public void CancelSubscription(string subscriptionId)
    {
        if (_iapIsReady)
        {
            IAPurchase.CancelSubscription(_iAPResult, subscriptionId);
        }
        else
        {
            Debug.LogError("Viveport IAP IsReady isn't Complete");
        }
    }

    class SampleIAPListener : IAPurchase.IAPurchaseListener
    {
        public override void OnSuccess(string pchCurrencyName)
        {
            _iapIsReady = true;
            MainThreadDispatcher.Instance().Enqueue(() => {
                Debug.Log("IAP IsReady Success");
                onIAPIsReadySuccess_s.Invoke(pchCurrencyName);
            });
        }

        public override void OnRequestSuccess(string pchPurchaseId)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPRequestSuccess_s.Invoke(pchPurchaseId);
            });
        }

        public override void OnPurchaseSuccess(string pchPurchaseId)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPurchaseSuccess_s.Invoke(pchPurchaseId);
            });
        }

        public override void OnQuerySuccess(IAPurchase.QueryResponse response)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPQuerySuccess_s.Invoke(response);
            });
        }

        public override void OnQuerySuccess(IAPurchase.QueryListResponse response)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPQueryListSuccess_s.Invoke(response.purchaseList);
            });
        }

        public override void OnBalanceSuccess(string pchBalance)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPBalanceSuccess_s.Invoke(pchBalance);
            });
        }

        public override void OnRequestSubscriptionSuccess(string pchSubscriptionId)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPRequestSubscriptionSuccess_s.Invoke(pchSubscriptionId);
            });
        }

        public override void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPRequestSubscriptionSuccess_s.Invoke(pchSubscriptionId);
            });
        }

        public override void OnSubscribeSuccess(string pchSubscriptionId)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPSubscribeSuccess_s.Invoke(pchSubscriptionId);
            });
        }

        public override void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPQuerySubscriptionSuccess_s.Invoke(subscriptionlist);
            });
        }

        public override void OnQuerySubscriptionListSuccess(IAPurchase.Subscription[] subscriptionlist)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPQuerySubscriptionSuccess_s.Invoke(subscriptionlist);
            });
        }

        public override void OnCancelSubscriptionSuccess(bool bCanceled)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPCancelSubscriptionSuccess_s.Invoke(bCanceled);
            });
        }

        public override void OnFailure(int nCode, string pchMessage)
        {
            MainThreadDispatcher.Instance().Enqueue(() => {
                onIAPFailure_s.Invoke(nCode, pchMessage);
            });
        }
    }
}
