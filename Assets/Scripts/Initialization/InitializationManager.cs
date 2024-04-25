using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardGame.SceneManagement;
using CardGame.ServiceManagement;
using CardGame.Singleton;
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
            ServiceLocator.Global.Get(out SceneLoader sceneLoader);
            if (sceneLoader != null) sceneLoader.LoadScene(SceneType.GameScene);
        }
    }
}
