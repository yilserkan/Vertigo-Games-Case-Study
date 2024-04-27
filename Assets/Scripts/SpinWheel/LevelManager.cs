using System;
using CardGame.CloudServices.InventoryService;
using CardGame.Inventory;
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
        public static event Action<LevelSlotData> OnRewardClaimed; 
        
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
            ServiceLocator.ForScene(this)?.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ForScene(this, false)?.Unregister(this);
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
            PlayerInventory.ClearWheelRewards();
        }
        
        private void HandleOnSpinWheelAnimCompleted(int slotIndex)
        {
            var slotData = _levelData.Levels[_currentStage.Value].SlotDatas[slotIndex];
            if (HasPlayerLost(slotData))
            {
                Debug.Log("Player Has Lost");
                OnPlayerHasLostEvent?.Invoke();
                return;
            }
            
            OnRewardClaimed?.Invoke(slotData);
        }

        private void HandleOnRewardParticlesCompleted()
        {
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
            PlayerInventory.DecreaseCurrencyLocally(CurrencyType.Gold, LostPanelUIManager.REVIVE_COST);
            var revivePlayerResponse = await SpinWheelCloudRequests.Revive();

            if (revivePlayerResponse.ReviveSuccessful)
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
            PlayerInventory.ClaimRewardsLocally();
            await SpinWheelCloudRequests.ClaimRewards();

            //TODO : Show Rewards
            StartGame();
        }

        private bool HasPlayerLost(LevelSlotData slotData)
        {
            Debug.Log("Item ID : " + slotData.ID);
            return (ItemType)slotData.Type == ItemType.Bomb;
        }
        
        private bool IsAtZoneLevel() => (LevelType)LevelData.Levels[_currentStage.Value].LevelType is LevelType.SafeZone or LevelType.SuperZone; 
        
        private void AddListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted += HandleOnSpinWheelAnimCompleted;
            SpinWheelRewardsManager.OnRewardParticlesCompleted += HandleOnRewardParticlesCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent += HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent += HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked += HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked += ShowNextStage;
        }

        private void RemoveListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted -= HandleOnSpinWheelAnimCompleted;
            SpinWheelRewardsManager.OnRewardParticlesCompleted -= HandleOnRewardParticlesCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent -= HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent -= HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked -= HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked -= ShowNextStage;
        }
    }
}