using System;
using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.Utils;
using CardGame.Zones;
using UnityEngine;

namespace CardGame.SpinWheel
{
    public class LevelManager : PostStartMono
    {
        private GetLevelResponse _levelData;
        private Observable<int> _currentStage;
        public int CurrentStage => _currentStage.Value;
        public GetLevelResponse LevelData => _levelData;
        
        public static event Action OnStartGame;
        public static event Action OnQuitGame;
        public static event Action OnPlayerHasLostEvent;
        public static event Action<int> OnShowNextStage;
        public static event Action OnShowZonePanel;
        
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
            ServiceLocator.ForScene(this).Register(this);
        }

        protected override void Start()
        {
            base.Start();
            _currentStage = new Observable<int>(0);
        }

        protected override void PostStart()
        {
            base.PostStart();
            StartGame();
        }

        public async void StartGame()
        {
            ResetLevel();
            _levelData = await SpinWheelCloudRequests.GetLevelData();
            OnStartGame?.Invoke();
        }

        private void ResetLevel()
        {
            _currentStage.Value = 0;
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

            // TODO : Check for game end
            
            if (IsAtZoneLevel())
            {
                OnShowZonePanel?.Invoke();
                return;
            }
            
            ShowNextStage();
        }

        private void ShowNextStage()
        {
            _currentStage.Value++;
            // _spinWheelManager.ShowStage(_currentStage.Value);
            OnShowNextStage?.Invoke(_currentStage.Value);
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

        private bool HasPlayerLost(int slotIndex)
        {
            var slotData = _levelData.Levels[_currentStage.Value].SlotDatas[slotIndex];
            Debug.Log("Item ID : " + slotData.ID);
            return (ItemType)slotData.Type == ItemType.Bomb;
        }
        
        private bool IsAtZoneLevel() => (LevelType)LevelData.Levels[_currentStage.Value].LevelType is LevelType.SafeZone or LevelType.SuperZone; 
        
        private void AddListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted += HandleOnSpinWheelAnimCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent += HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent += HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked += HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked += ShowNextStage;
        }

        private void RemoveListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted -= HandleOnSpinWheelAnimCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent -= HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent -= HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked -= HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked -= ShowNextStage;
        }
    }
}