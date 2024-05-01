using System;
using CardGame.Inventory;
using CardGame.SpinWheel;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PanelAnimationHelper))]
public class WinPanelUIManager : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private GameObject _winPanelParent;
    [SerializeField] private PanelAnimationHelper _panelAnimationHelper;
    
    public static event Action OnPlayAgainButtonClicked;
    
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void OnValidate()
    {
        _panelAnimationHelper = GetComponent<PanelAnimationHelper>();
    }

    private void ShowWinPanel()
    {
        EnableWinPanel(true);
        SetPlayAgainButtonInteractable(false);
        _panelAnimationHelper.PlayOpeningAnimation((() => SetPlayAgainButtonInteractable(true)));
    }

    private void SetPlayAgainButtonInteractable(bool interactable)
    {
        _playAgainButton.interactable = interactable;
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