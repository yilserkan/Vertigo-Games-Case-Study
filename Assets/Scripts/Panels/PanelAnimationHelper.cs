using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.Panels
{
    public class PanelAnimationHelper : MonoBehaviour
    {
        [SerializeField] private Image _darkBgImage;
        [SerializeField] private Transform _parent;
        [SerializeField] private Transform[] _buttonsTransforms;
        
        private const float OPENING_ANIM_DURATION = .25f;
        private const float BUTTON_ANIM_DELAY = .2f;
        private Color _openingAnimBGStartColor = new Color(0, 0, 0, 0);
        private Color _openingAnimBgEndColor = new Color(0, 0, 0, .975f);

        private Action _onAnimCompletedAction;
        
        public void PlayOpeningAnimation(Action onCompleted)
        {
            _onAnimCompletedAction = onCompleted;
            SetAnimStartValues();
            _parent.DOScale(1, OPENING_ANIM_DURATION)
                .SetEase(Ease.OutBack)
                .OnComplete(OnOpeningAnimationFinished);
            
            _darkBgImage.DOColor(_openingAnimBgEndColor, OPENING_ANIM_DURATION);
        }

        private void OnOpeningAnimationFinished()
        {
            if (_buttonsTransforms.Length == 0)
            {
                _onAnimCompletedAction?.Invoke();
            }
            
            PlayButtonAnimations();
        }
        
        private void PlayButtonAnimations()
        {
            for (int i = 0; i < _buttonsTransforms.Length; i++)
            {
                
                var tween = _buttonsTransforms[i].DOScale(1, OPENING_ANIM_DURATION).SetEase(Ease.OutBack).SetDelay(BUTTON_ANIM_DELAY * (i + 1));

                if (i == _buttonsTransforms.Length - 1)
                {
                    tween.OnComplete(() => _onAnimCompletedAction?.Invoke());
                }
            }
        }

        private void SetAnimStartValues()
        {
            _darkBgImage.color = _openingAnimBGStartColor;
            _parent.localScale = Vector3.one / 2;
            
            if (_buttonsTransforms != null)
            {
                for (int i = 0; i < _buttonsTransforms.Length; i++)
                {
                    _buttonsTransforms[i].localScale = Vector3.zero;
                }
            }
        }
    }
}