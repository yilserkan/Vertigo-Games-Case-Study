using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.SpinWheel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ZonePanelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private Button _claimRewardsButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Image _zonePanelBgImage;
    [SerializeField] private TextMeshProUGUI _zoneText;
    [SerializeField] private ZonePanelUIAssets zonePanelUIAssets;

    public static event Action OnClaimRewardsButtonClicked;
    
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void HandleOnContinueButtonClicked()
    {
        EnableClaimRewardsPanel(false);
    }

    private void HandleOnClaimRewardsButtonClicked()
    {
        OnClaimRewardsButtonClicked?.Invoke();
        EnableClaimRewardsPanel(false);
    }
    
    private void HandleOnShowClaimRewardsPanel(LevelType levelType)
    {
        SetUI(levelType);
        EnableClaimRewardsPanel(true);
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
        LevelManager.OnShowClaimRewardsPanel += HandleOnShowClaimRewardsPanel;
    }

    private void RemoveListeners()
    {
        _claimRewardsButton.onClick.RemoveListener(HandleOnClaimRewardsButtonClicked);
        _continueButton.onClick.RemoveListener(HandleOnContinueButtonClicked);
        LevelManager.OnShowClaimRewardsPanel -= HandleOnShowClaimRewardsPanel;
    }
}