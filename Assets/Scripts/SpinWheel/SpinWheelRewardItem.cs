using System;
using CardGame.Extensions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SpinWheel
{
    public class SpinWheelRewardItem : MonoBehaviour
    {
        [SerializeField] private RectTransform _rewardIconRect;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardAmount;

        private float _currentAmount;
        private const float TEXT_ANIM_DURATION = .25f;
        private const float SPAWN_ANIM_DURATION = .25f;
        
        private void OnValidate()
        {
            if (_rewardIcon != null)
            {
                _rewardIconRect = _rewardIcon.GetComponent<RectTransform>();
            }
        }
        
        public void SetReward(Sprite sprite)
        {
            _currentAmount = 0;
            _rewardAmount.text = $"{_currentAmount}";
            _rewardIcon.sprite = sprite;
            PlaySpawnAnimation();
        }

        private void PlaySpawnAnimation()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, SPAWN_ANIM_DURATION).SetEase(Ease.OutBack);
        }
        
        public void UpdateAmount(float amount)
        {
            _currentAmount += amount;
            _rewardAmount.CustomDOText(_currentAmount.ToString(), TEXT_ANIM_DURATION);
        }

        public RectTransform GetImageRect()
        {
            return _rewardIconRect;
        }
    }
}