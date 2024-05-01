using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Inventory;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardGame.Zones
{

    public class ZonePanelUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private Button _claimRewardsButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Image _zonePanelBgImage;
        [SerializeField] private TextMeshProUGUI _zoneText;
        [SerializeField] private ZonePanelUIAssets zonePanelUIAssets;
  
        
        public static event Action OnClaimRewardsButtonClicked;
        public static event Action OnContinuButtonClicked;
        
        private void OnEnable()
        {
            AddListeners();
        }
    
        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Awake()
        {
            ServiceLocator.For(this)?.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.For(this, false)?.Unregister(this);
        }

        private void HandleOnContinueButtonClicked()
        {
            EnableClaimRewardsPanel(false);
            OnContinuButtonClicked?.Invoke();
        }
    
        private void HandleOnClaimRewardsButtonClicked()
        {
            OnClaimRewardsButtonClicked?.Invoke();
            EnableClaimRewardsPanel(false);
        }
        
        public void ShowZonePanel(LevelType levelType)
        {
            SetUI(levelType);
            EnableClaimRewardsPanel(true);
            SetButtonInteractables(false);
        }

        public void SetButtonInteractables(bool interactable)
        {
            _continueButton.interactable = interactable;
            _claimRewardsButton.interactable = interactable;
        }
    
        private void SetUI(LevelType levelType)
        {
            _zonePanelBgImage.color = zonePanelUIAssets.GetBgSpriteColor(levelType);
            _zoneText.text = zonePanelUIAssets.GetZoneText(levelType);
            _zoneText.color = zonePanelUIAssets.GetTextColor(levelType);
        }
        
        private void EnableClaimRewardsPanel(bool enable)
        {
            _parent.SetActive(enable);
        }
        
        private void AddListeners()
        {
            _claimRewardsButton.onClick.AddListener(HandleOnClaimRewardsButtonClicked);
            _continueButton.onClick.AddListener(HandleOnContinueButtonClicked);
        }
    
        private void RemoveListeners()
        {
            _claimRewardsButton.onClick.RemoveListener(HandleOnClaimRewardsButtonClicked);
            _continueButton.onClick.RemoveListener(HandleOnContinueButtonClicked);
        }
    }
}
