using UnityEngine;

namespace CardGame.Singleton
{
    public abstract class AbstractMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static bool HasInstance() => _instance != null;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var foundObjects = FindObjectsOfType<T>();
            
                if (foundObjects.Length > 0)
                {
                    _instance = foundObjects[0];

                    if (foundObjects.Length > 1)
                    {
                        Debug.LogError($"There are multiple Singletons of type {nameof(T)}");
                    }
                }

                GameObject obj = new GameObject($"{typeof(T)}");
                var newInstance = obj.AddComponent<T>();
                _instance = newInstance;
                return _instance;
            }
        }

        public virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
