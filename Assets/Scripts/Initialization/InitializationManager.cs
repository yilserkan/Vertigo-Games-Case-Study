using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.SceneManagement;
using CardGame.ServiceManagement;
using CardGame.Singleton;
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
        public struct userAttributes {}
        public struct appAttributes {}
        
        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            // initialize Unity's authentication and core services, however check for internet connection
            // in order to fail gracefully without throwing exception if connection does not exist
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeRemoteConfigAsync();
            }

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
            Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config);
            Debug.Log("PlayerId : " + AuthenticationService.Instance.PlayerId);
            ServiceLocator.LazyGlobal
                .Get(out SceneLoader sceneLoader)
                .Get(out ScriptableManagerInitializerMono scriptableManagerInitializer);
            
            if (scriptableManagerInitializer != null)
            {
                await scriptableManagerInitializer.InitializeScriptableManagers();
            }
            
            await PlayerInventory.Initialize();
                
            sceneLoader.OrNull()?.LoadScene(SceneType.GameScene);
        }
    }
}
