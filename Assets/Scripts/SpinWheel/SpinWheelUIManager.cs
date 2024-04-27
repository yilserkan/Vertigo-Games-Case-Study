using System;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SpinWheel
{
    public class SpinWheelUIManager : MonoBehaviour
    {
        [SerializeField] private Button _spinButton;
        [SerializeField] private Image _wheelBodyImage;
        [SerializeField] private Image _wheeIndicatorImage;
        [SerializeField] private SpinWheelUIAssets _wheelUIAssets;
        
        private SpinWheelManager _spinWheelManager;
        
        private void Awake()
        {
            ServiceLocator.For(this)?.Register<SpinWheelUIManager>(this);
        }

        private void Start()
        {
            ServiceLocator.For(this)?.Get(out _spinWheelManager);
        }

        private void OnDestroy()
        {
            ServiceLocator.For(this, false)?.Unregister(this);
        }

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void SetWheelUI(LevelType levelType)
        {
            if (_wheelUIAssets.TryGetWheelBodySprite(levelType, out var bodySprite))
            {
                _wheelBodyImage.sprite = bodySprite;
            }

            if (_wheelUIAssets.TryGetWheelIndicatorSprite(levelType, out var indicatorSprite))
            {
                _wheeIndicatorImage.sprite = indicatorSprite;
            }
        }

        private void HandleOnSpinButtonClicked()
        {
            SetSpinButtonInteractable(false);
            _spinWheelManager.SpinWheel();
        }

        public void SetSpinButtonInteractable(bool interactable)
        {
            _spinButton.interactable = interactable;
        }
        
        private void AddListeners()
        {
            _spinButton.onClick.AddListener(HandleOnSpinButtonClicked);
        }

        private void RemoveListeners()
        {
            _spinButton.onClick.AddListener(HandleOnSpinButtonClicked);
        }
    }
}