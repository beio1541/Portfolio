﻿using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_2012_2
using System.Threading.Tasks;
#endif

using UnityEngine;

namespace Framework.Network.Web
{
    public class WebNetwork : Framework.Support.Singleton<WebNetwork>, Framework.Interface.ISingleton
    {
#if UNITY_2012_2
#else
        Queue<Request> requests = null;
        Coroutine coroutine = null;
#endif
        protected override void OnInitialize()
        {
#if UNITY_2012_2
#else
            requests = new Queue<Request>();
#endif
        }
        protected override void OnDestroy()
        {
        }
#if UNITY_2012_2
        async public Task SendRequest(Request webRequest)
        {
            await webRequest.SendAync();
            if (webRequest.IsError)
                Debugger.LogError($"RECV : E_FAIL {nameof(webRequest.URL)}.{webRequest.URL}, {nameof(webRequest.ResponseCode)}.{webRequest.ResponseCode}, {webRequest.ErrorString}");
            else
                Debugger.Log($"RECV : S_OK {nameof(webRequest.URL)}.{webRequest.URL}, {nameof(webRequest.ResponseCode)}.{webRequest.ResponseCode}");
            try
            {
                webRequest.Invoke();
            }
            catch (Exception e) { Debugger.LogException(e); }
        }
#else
        public void SendRequest(Request webRequest)
        {
            requests.Enqueue(webRequest);
            if (coroutine == null)
                coroutine = behaviour.StartCoroutine(CoRequestUpdate());
        }
        IEnumerator CoRequestUpdate()
        {
            while (requests.Count > 0)
            {
                Request info = requests.Dequeue();
                if (info == null) continue;
                yield return info.SendAync();
                if (info.isError)
                    Common.Debugger.LogError($"RECV : E_FAIL {nameof(info.URL)}.{info.URL}, {nameof(info.responseCode)}.{info.responseCode}, {info.errorString}");
                else
                    Common.Debugger.Log($"RECV : S_OK {nameof(info.URL)}.{info.URL}, {nameof(info.responseCode)}.{info.responseCode}");
                try
                {
                    info.Invoke();
                }
                catch (Exception e) { Common.Debugger.LogException(e); }
            }
            behaviour.StopCoroutine( coroutine);
            coroutine = null;
        }
    }
#endif
}