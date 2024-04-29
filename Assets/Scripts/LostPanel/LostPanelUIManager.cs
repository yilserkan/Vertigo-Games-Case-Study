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

public class LostPanelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _reviveCurrencyAmountText;
    
    public static event Action OnRestartButtonClickedEvent;
    public static event Action OnReviveButtonClickedEvent;

    private int _reviveCost;
    
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
        LevelManager levelManager = null;
        ServiceLocator.Global.OrNull()?.Get(out levelManager);
        if (levelManager != null)
        {
            _reviveCost = levelManager.GetReviveCost();
        }
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

    private void HandleOnPlayerHasLost()
    {
        EnableLostPanel(true);
    }

    private void EnableLostPanel(bool enable)
    {
        UpdateReviveButton();
        _parent.SetActive(enable);
    }

    private void UpdateReviveButton()
    {
        var hasPlayerEnoughMoney = PlayerInventory.GetCurrencyAmount(CurrencyType.Gold) >= _reviveCost;
        _reviveButton.interactable = hasPlayerEnoughMoney;
        _reviveCurrencyAmountText.text = $"{_reviveCost}";
    }
    
    private void AddListeners()
    {
        _reviveButton.onClick.AddListener(HandleOnReviveButtonClicked);
        _restartButton.onClick.AddListener(HandleOnRestartButtonClicked);
        LevelManager.OnPlayerHasLostEvent += HandleOnPlayerHasLost;
    }
    
    private void RemoveListeners()
    {
        _reviveButton.onClick.RemoveListener(HandleOnReviveButtonClicked);
        _restartButton.onClick.RemoveListener(HandleOnRestartButtonClicked);
        LevelManager.OnPlayerHasLostEvent -= HandleOnPlayerHasLost;
    }
}
