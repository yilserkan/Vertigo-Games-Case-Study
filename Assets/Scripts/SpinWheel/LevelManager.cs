using System;
using CardGame.Items;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.SpinWheel
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelManager _spinWheelManager;
        [SerializeField] private ZonePanelManager _zonePanelManager;
        private GetLevelResponse _levelData;
        private int _currentStage;

        public static event Action OnPlayerHasLostEvent;
        public static event Action<int> OnShowNextStage;

        private Observable<int> _nextSafeZone;
        
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
            StartGame();
        }

        public async void StartGame()
        {
            ResetLevel();
            _levelData = await SpinWheelCloudRequests.GetLevelData();
            _spinWheelManager.InitializeLevel(_levelData);
            _zonePanelManager.Initialize(_levelData);
            _spinWheelManager.ShowStage(_currentStage);
        }

        private void ResetLevel()
        {
            _currentStage = 0;
        }
        
        private void HandleOnSpinWheelAnimCompleted(int slotIndex)
        {
            if (HasPlayerLost(slotIndex))
            {
                Debug.Log("Player Has Lost");
                OnPlayerHasLostEvent?.Invoke();
                return;
            }
            // TODO : Show UI Animation
            
            ShowNextStage();
        }

        private void ShowNextStage()
        {
            _currentStage++;
            _spinWheelManager.ShowStage(_currentStage);
            OnShowNextStage?.Invoke(_currentStage);
        }
     
        private bool HasPlayerLost(int slotIndex)
        {
            var slotData = _levelData.LevelData[_currentStage].SlotDatas[slotIndex];
            Debug.Log("Item ID : " + slotData.ID);
            return (ItemType)slotData.Type == ItemType.Bomb;
        }

        private async void HandleOnRestartButtonClicked()
        {
            await SpinWheelCloudRequests.GiveUp();
            StartGame();
        }

        private async void HandleOnReviveButtonClicked()
        {
            var revivePlayerResponse = await SpinWheelCloudRequests.Revive();

            if (revivePlayerResponse.ReviveSuccessfull)
            {
                ShowNextStage();
            }
            else
            {
                StartGame();
            }
        }

        private async void HandleOnClaimRewardsButtonClicked()
        {
            await SpinWheelCloudRequests.ClaimRewards();
            
            //TODO : Show Rewards
            StartGame();
        }
        
        
        private void AddListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted += HandleOnSpinWheelAnimCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent += HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent += HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked += HandleOnClaimRewardsButtonClicked;
        }

        private void RemoveListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted -= HandleOnSpinWheelAnimCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent -= HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent -= HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked -= HandleOnClaimRewardsButtonClicked;
        }
    }
}