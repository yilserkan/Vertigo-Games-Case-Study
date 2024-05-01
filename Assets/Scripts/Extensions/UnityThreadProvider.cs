using System;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using UnityEngine;

namespace CardGame.Utils
{
    // This class Provides Unity Thread to static classes so they can also use the thread for 
    // unity thread specific calls like starting a coroutine.
    public class UnityThreadProvider : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Global.OrNull()?.Unregister(this);
        }
    }
}