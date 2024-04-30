using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.NetworkConnection;
using CardGame.SceneManagement;
using CardGame.ServiceManagement;
using CardGame.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CardGame.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject _loadingHelper;
        
        public struct userAttributes {}
        public struct appAttributes {}
        
        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            NetworkConnectionHelper connectionHelper = null;
            ServiceLocator.Global.OrNull()?.Get(out connectionHelper);
            if (connectionHelper == null)
            {
                Debug.LogError("Network Connection Helper Not Found!");
                return;
            }
            
            while (!connectionHelper.HasInternetConnection)
            {
                Debug.LogWarning("Trying to Reconnect...");
                await Task.Delay(500);
            }
            
            Debug.Log("Started Initializing Unity Services...");
            await InitializeRemoteConfigAsync();

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }
        
        async Task InitializeRemoteConfigAsync()
        {
            // initialize handlers for unity game services
            await UnityServices.InitializeAsync();
            
            // remote config requires authentication for managing environment information
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        async void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            Debug.Log("Unity Services Initialized ");
            Debug.Log("PlayerId : " + AuthenticationService.Instance.PlayerId);
            ServiceLocator.LazyGlobal
                .Get(out SceneLoader sceneLoader)
                .Get(out ScriptableManagerInitializerMono scriptableManagerInitializer);
            
            if (scriptableManagerInitializer != null)
            {
                await scriptableManagerInitializer.InitializeScriptableManagers();
            }
            
            await PlayerInventory.Initialize();

            await _loadingHelper.InstantiateAddressableAsync();
            sceneLoader.OrNull()?.LoadScene(SceneType.GameScene);
        }
    }
}
