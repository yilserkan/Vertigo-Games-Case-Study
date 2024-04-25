using System;
using UnityEngine;

namespace CardGame.Utils
{
    public class PersistentMono : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
