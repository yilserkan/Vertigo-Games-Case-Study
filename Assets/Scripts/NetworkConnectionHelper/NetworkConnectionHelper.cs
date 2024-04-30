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
        public bool HasInternetConnection => _hasInternetConnection != null && _hasInternetConnection.Value;
        
        private void Awake()
        {
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
        }
    
        private void Start()
        {
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
            if (_hasInternetConnection == null)
            {
                _hasInternetConnection = new Observable<bool>(hasConnection && !_forceDisableInternetConnection, connectionUIManager);
            }
            else
            {
                _hasInternetConnection.Value = hasConnection && !_forceDisableInternetConnection;
            }
#else
            Debug.Log("Has Internet Connection : " + hasConnection);
            if (_hasInternetConnection == null)
            {
                _hasInternetConnection = new Observable<bool>(hasConnection, connectionUIManager);
            }
            else
            {
                _hasInternetConnection.Value = hasConnection;
            }
#endif
        }
    }
}
