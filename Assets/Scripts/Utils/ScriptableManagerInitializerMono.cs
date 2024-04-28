using System;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.Utils
{
    public class ScriptableManagerInitializerMono : MonoBehaviour
    {
        [SerializeField] private AbstractScriptableManagerBase[] _abstractScriptableManagerArray;
        [SerializeField] private AssetReferenceScriptableManagerBase[] _addressableScriptableManagerArray;
        
        private void Awake()
        {
            ServiceLocator.LazyGlobal.Register(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Global.OrNull()?.Unregister(this);
        }

        public async Task InitializeScriptableManagers()
        {
            for (int i = 0; i < _abstractScriptableManagerArray.Length; i++)
            {
                _abstractScriptableManagerArray[i].Initialize();
            }
            
            for (int i = 0; i < _addressableScriptableManagerArray.Length; i++)
            {
                var scriptableManager = await _addressableScriptableManagerArray[i].LoadAddressableAsync();
                scriptableManager.Initialize();
            }
        }
    }
}