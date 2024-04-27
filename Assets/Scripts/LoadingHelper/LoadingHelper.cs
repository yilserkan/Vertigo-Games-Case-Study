using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SceneManagement
{
    public class LoadingHelper : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private Image _loadingBarFillImage;

        private void Awake()
        {
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
            EnableLoadingPanel(false);
        }

        private void OnDestroy()
        {
            ServiceLocator.Global.OrNull()?.Unregister(this);
        }

        public void EnableLoadingPanel(bool enabled)
        {
            gameObject.SetActive(enabled);
        }
    
        public void Report(float value)
        {
            UpdateLoadingBar(value);
        }
   
        private void UpdateLoadingBar(float value)
        {
            _loadingBarFillImage.fillAmount = value;
        }
    }
}
