using System;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using DG.Tweening;
using UnityEngine;

namespace CardGame.SceneManagement
{
    public class RequestLoadingHelper : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private RectTransform _requestLoadingTransform;

        private const float ROTATE_ANIM_SINGLE_LOOP_DURATION = .5f;
        
        private void Awake()
        {
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Global.OrNull()?.Register(this);
        }

        public void EnableRequestLoadingPanel(bool isEnabled)
        {
            _parent.SetActive(isEnabled);

            if (isEnabled)
            {
               StartLoadingAnimation();
            }
            else
            {
                StopLoadingAnimation();
            }
        }

        private void StartLoadingAnimation()
        {
            _requestLoadingTransform
                .DORotate(new Vector3(0, 0, -359), ROTATE_ANIM_SINGLE_LOOP_DURATION, RotateMode.WorldAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }
        
        private void StopLoadingAnimation()
        {
            _requestLoadingTransform.DOKill();
        }
    }
}