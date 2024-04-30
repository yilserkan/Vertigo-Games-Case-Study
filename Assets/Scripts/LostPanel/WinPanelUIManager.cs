using System;
using CardGame.SpinWheel;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelUIManager : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private GameObject _winPanelParent;
    
    public static event Action OnPlayAgainButtonClicked;
    
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void ShowWinPanel()
    {
        EnableWinPanel(true);
    }
    
    private void HandleOnPlayAgainButtonClicked()
    {
        OnPlayAgainButtonClicked?.Invoke();
        EnableWinPanel(false);
    }

    private void EnableWinPanel(bool enable)
    {
        _winPanelParent.SetActive(enable);
    }
    
    private void AddListeners()
    {
        _playAgainButton.onClick.AddListener(HandleOnPlayAgainButtonClicked);
        LevelManager.OnPlayerCompletedAllLevels += ShowWinPanel;
    }

    private void RemoveListeners()
    {
        _playAgainButton.onClick.RemoveListener(HandleOnPlayAgainButtonClicked);
        LevelManager.OnPlayerCompletedAllLevels -= ShowWinPanel;
    }
}