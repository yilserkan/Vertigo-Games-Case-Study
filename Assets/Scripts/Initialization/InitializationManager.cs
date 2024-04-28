using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.SceneManagement;
using CardGame.ServiceManagement;
using CardGame.Singleton;
using CardGame.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CardGame.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            ServiceLocator.LazyGlobal
                .Get(out SceneLoader sceneLoader)
                .Get(out ScriptableManagerInitializerMono scriptableManagerInitializer);

            if (scriptableManagerInitializer != null)
            {
                await scriptableManagerInitializer.InitializeScriptableManagers();
            }
       
            sceneLoader.OrNull()?.LoadScene(SceneType.GameScene);
            
            PlayerInventory.Initialize();
        }
    }
}
