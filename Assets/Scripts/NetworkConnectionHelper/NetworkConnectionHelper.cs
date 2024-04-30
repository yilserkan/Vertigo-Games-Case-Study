using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using CardGame.Utils;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace CardGame.NetworkConnection
{
    public class NetworkConnectionHelper : MonoBehaviour
    {
        [SerializeField] private NetworkConnectionUIManager connectionUIManager;
        [SerializeField] private int _pingInterval;
        [SerializeField] private bool _forceDisableInternetConnection;
        
        private float _time;
        public Observable<bool> _hasInternetConnection;
        public bool HasInternetConnection => _hasInternetConnection.Value;
        
        private void Awake()
        {
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
        }
    
        private void Start()
        {
            _hasInternetConnection = new Observable<bool>(false, connectionUIManager);
            _time = _pingInterval;
        }
    
        private void Update()
        {
            if (_time <= 0f)
            {
                PingInternet();
                _time = _pingInterval;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
        
        private void PingInternet()
        {
            StartCoroutine(GetRequest("https://www.google.com/"));
        }
        
        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                var hasConnection = webRequest.result == UnityWebRequest.Result.Success;
                SetHasConnection(hasConnection);
            }
        }

        private void SetHasConnection(bool hasConnection)
        {
#if UNITY_EDITOR
            Debug.Log("Has Internet Connection : " + (hasConnection && !_forceDisableInternetConnection)) ;
            _hasInternetConnection.Value = hasConnection && !_forceDisableInternetConnection;
#else
            Debug.Log("Has Internet Connection : " + hasConnection);
            _hasInternetConnection.Value = hasConnection;
#endif
        }

        // public bool CheckInternetConnection()
        // {
        //     _hasInternetConnection.Value = GetInternetConnection();
        //     return _hasInternetConnection.Value;
        // }
        
        // Dont Check for Force Disable Internet Connection in builds
//         private bool GetInternetConnection()
//         {
// #if UNITY_EDITOR
//             var hasConnection = Utilities.CheckForInternetConnection();
//             Debug.Log("Has Internet Connection : " + (hasConnection && !_forceDisableInternetConnection)) ;
//             return hasConnection && !_forceDisableInternetConnection;
// #else
//             var hasConnection = Utilities.CheckForInternetConnection();
//             Debug.Log("Has Internet Connection : " + hasConnection);
//             return hasConnection;
// #endif
//         }
    }
}
