using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Panels
{
    
    [RequireComponent(typeof(PanelAnimationHelper))]
    public class LostPanelUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private Button _reviveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _reviveCurrencyAmountText;
        [SerializeField] private PanelAnimationHelper _panelAnimationHelper;
        
        public static event Action OnRestartButtonClickedEvent;
        public static event Action OnReviveButtonClickedEvent;

        private LevelManager _levelManager;
        
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Start()
        {
            ServiceLocator.ForScene(this).OrNull()?.Get(out _levelManager);
        }

        private void OnValidate()
        {
            _panelAnimationHelper = GetComponent<PanelAnimationHelper>();
        }

        private void HandleOnRestartButtonClicked()
        {
            OnRestartButtonClickedEvent?.Invoke();
            EnableLostPanel(false);
        }

        private void HandleOnReviveButtonClicked()
        {
            OnReviveButtonClickedEvent?.Invoke();
            EnableLostPanel(false);
        }

        private void HandleOnGameLost()
        {
            EnableLostPanel(true);
        }

        private void EnableLostPanel(bool enable)
        {
            UpdateReviveButton();
            _parent.SetActive(enable);
            SetButtonInteractables(false);
            _panelAnimationHelper.PlayOpeningAnimation((() => SetButtonInteractables(true)));
        }

        private void SetButtonInteractables(bool interactable)
        {
            _reviveButton.interactable = interactable;
            _restartButton.interactable = interactable;
        }

        private void UpdateReviveButton()
        {
            var reviveCost = _levelManager.GetReviveCost();
            var hasPlayerEnoughMoney = PlayerInventory.GetCurrencyAmount(CurrencyType.Gold) >= reviveCost;
            _reviveButton.interactable = hasPlayerEnoughMoney;
            _reviveCurrencyAmountText.text = $"{reviveCost}";
        }
        
        private void AddListeners()
        {
            _reviveButton.onClick.AddListener(HandleOnReviveButtonClicked);
            _restartButton.onClick.AddListener(HandleOnRestartButtonClicked);
            LevelManager.OnGameLost += HandleOnGameLost;
        }
        
        private void RemoveListeners()
        {
            _reviveButton.onClick.RemoveListener(HandleOnReviveButtonClicked);
            _restartButton.onClick.RemoveListener(HandleOnRestartButtonClicked);
            LevelManager.OnGameLost -= HandleOnGameLost;
        }
    }
}