using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame.ServiceManagement
{
    public class ServiceManager
    {
        private Dictionary<Type, object> _registeredServices = new Dictionary<Type, object>();

        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if (_registeredServices.ContainsKey(type))
            {
                return _registeredServices[type] as T;
            }

            return null;
        }

        public bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);
            if (_registeredServices.ContainsKey(type))
            {
                service = _registeredServices[type] as T;
                return true;
            }

            service = null;
            return false;
        }
        
        public void Register<T>(T service)
        {
            Type type = typeof(T);
            if (!_registeredServices.TryAdd(type, service))
            {
                Debug.LogWarning($"Service of type {type} has already been registered");
            }
        }

        public void Unregister<T>(T service)
        {
            Type type = typeof(T);
            if (!_registeredServices.ContainsKey(type))
            {
                Debug.LogWarning($"Trying to unregister service of type {type} when it is not valid in Registered Services");
                return;
            }

            _registeredServices.Remove(type);
        }
    }
}