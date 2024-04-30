using System;
using CardGame.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public class SpinWheelParticleItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _particleImage;
        [SerializeField] private float _radius;
        [SerializeField] private float _scaleAnimDuration;
        [SerializeField] private float _moveToTargerRewardAnimDuration;

        private RectTransform _targetRewardItemRect;
        public event Action OnParticleCompletedEvent;
        
        private void OnValidate()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void InitializeAndStartAnimation(RectTransform rewardItemRect, Sprite sprite)
        {
            _particleImage.sprite = sprite;
            _targetRewardItemRect = rewardItemRect;
            _particleImage.enabled = true;
            PlayScaleAnimation();
        }
        
        private void PlayScaleAnimation()
        {
            var targetPoint = GetRandomPointInCircle();
            _rectTransform.localScale = Vector3.zero;
            _rectTransform.anchoredPosition = Vector2.zero;

            _rectTransform.DOScale(Vector3.one, _scaleAnimDuration);
            _rectTransform.DOAnchorPos(targetPoint, _scaleAnimDuration).OnComplete(PlayMoveToTargetRewardAnimation);
        }

        private void PlayMoveToTargetRewardAnimation()
        {
            _rectTransform.DOAnchorPos(_targetRewardItemRect.ToAnchoredPosOfRect(_rectTransform), _moveToTargerRewardAnimDuration).OnComplete(OnParticleCompleted);
        }
        
        private void OnParticleCompleted()
        {
            OnParticleCompletedEvent?.Invoke();
            Addressables.ReleaseInstance(gameObject);
        }
        
        private Vector2 GetRandomPointInCircle()
        {
            return Random.insideUnitCircle * _radius;
        }
    }
}