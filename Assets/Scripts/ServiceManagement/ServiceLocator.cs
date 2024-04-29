using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame.ServiceManagement
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global;
        private static Dictionary<Scene, ServiceLocator> _sceneContainers = new Dictionary<Scene, ServiceLocator>();
        private ServiceManager _serviceManager = new ServiceManager();
    
        public static ServiceLocator LazyGlobal
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

        public static ServiceLocator Global => _global;

        public static ServiceLocator For(MonoBehaviour mb, bool createGlobalIfNotExists = true)
        {
            // Try get GameObject scoped service locator
            var serviceLocator = mb.GetComponentInParent<ServiceLocator>();
            if (serviceLocator != null)
            {
                return serviceLocator;
            }

            // Try get Scene scoped service locator
            var sceneServiceLocator = ForScene(mb, false);
            if (sceneServiceLocator != null)
            {
                return sceneServiceLocator;
            }
            
            // Try get Global scoped service locator
            if (createGlobalIfNotExists && LazyGlobal != null)
            {
                  return LazyGlobal; 
            }

            return Global;
        }

        public static ServiceLocator ForScene(MonoBehaviour mb, bool createIfNotExists = true)
        {
            Scene scene = mb.gameObject.scene;
            return ForScene(scene, createIfNotExists);
        }
        
        public static ServiceLocator ForActiveScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            return ForScene(scene, false);
        }
        
        private static ServiceLocator ForScene(Scene scene, bool createIfNotExists = true)
        {
            if (_sceneContainers.TryGetValue(scene, out var sceneServiceLocator))
            {
                if (sceneServiceLocator != null)
                {
                    return sceneServiceLocator;
                }

                _sceneContainers.Remove(scene);
            }

            if (!createIfNotExists) { return null; }
            
            var gameObject = new GameObject($"{scene.name} Service Locator");
            var serviceLocator = gameObject.AddComponent<ServiceLocator>();
            _sceneContainers.Add(scene, serviceLocator);

            return serviceLocator;
        }

        public static void RemoveScene(Scene scene)
        {
            if (!_sceneContainers.ContainsKey(scene)) { return; }

            _sceneContainers.Remove(scene);
        }
        
        public ServiceLocator Get<T>(out T service) where T : class
        {
            // GameObject scoped service locator
            if (_serviceManager.TryGet<T>(out service)) return this;

            // Scene Scoped Service Locator
            var sceneServiceLocator = ForScene(this, false);
            if (sceneServiceLocator != null && sceneServiceLocator.TryGet(out service))
            {
                return sceneServiceLocator;
            }
            
            // Global scoped service locator
            if (LazyGlobal != null && LazyGlobal.TryGet(out service)) return LazyGlobal;
            
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
