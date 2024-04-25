using System;
using System.Collections;
using UnityEngine;

namespace CardGame.ServiceManagement
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global;
        private ServiceManager _serviceManager = new ServiceManager();
    
        public static ServiceLocator Global
        {
            get
            {
                if (_global != null) return _global;
                
                var gameObject = new GameObject("Global Service Locator");
                var serviceLocator = gameObject.AddComponent<ServiceLocator>();
                _global = serviceLocator;
                DontDestroyOnLoad(gameObject);
                return _global;
            }
        }

        public static ServiceLocator For(MonoBehaviour mb)
        {
            // Try get GameObject scoped service locator
            var serviceLocator = mb.GetComponentInParent<ServiceLocator>();
            if (serviceLocator != null)
            {
                return serviceLocator;
            }
            
            // Try get global scoped service locator
            if (Global != null)  return Global; 
            
            return null;
        }
        
        public ServiceLocator Get<T>(out T service) where T : class
        {
            // GameObject scoped service locator
            if (_serviceManager.TryGet<T>(out service)) return this;
            // Global scoped service locator
            if (Global != null && Global.TryGet(out service)) return Global;
            return null;
        }
        
        public bool TryGet<T>(out T service) where T : class
        {
            return _serviceManager.TryGet(out service);
        }
        
        public void Register<T>(T service)
        {
            _serviceManager.Register(service);
        }
    
        public void Unregister<T>(T service)
        {
            _serviceManager.Unregister(service);
        }
    }
}
