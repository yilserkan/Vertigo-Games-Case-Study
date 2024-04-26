using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.SpinWheel;
using UnityEngine;
using UnityEngine.UI;

public class LostPanelUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private Button _restartButton;

    public static event Action OnRestartButtonClickedEvent;
    public static event Action OnReviveButtonClickedEvent;
    
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
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
        _parent.SetActive(enable);
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
