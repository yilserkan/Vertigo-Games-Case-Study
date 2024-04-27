using System;
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
        }

        public void UpdateAmount(float amount)
        {
            _currentAmount += amount;
            _rewardAmount.text = $"{_currentAmount}";
        }

        public RectTransform GetImageRect()
        {
            return _rewardIconRect;
        }
    }
}