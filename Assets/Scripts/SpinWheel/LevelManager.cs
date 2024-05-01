using System;
using System.Collections.Generic;
using CardGame.CloudServices;
using CardGame.CloudServices.EconomyService;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.Items;
using CardGame.Panels;
using CardGame.RemoteConfig;
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
        public static event Action OnGameLost;
        public static event Action OnGameCompleted;
        public static event Action<int> OnShowNextStage;
        public static event Action OnShowZonePanel;
        public static event Action<LevelSlotData> OnRewardClaimed; 
        
        private int _reviveCost;
        private int _revivedAmount;
        
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
            
            SpinWheelConfigData wheelData = null;
            ServiceLocator.Global.OrNull()?.Get(out wheelData);
            if (wheelData != null)
            {
                _reviveCost = wheelData.ConfigData.ReviveCost;
            }
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
            _revivedAmount = 0;
            _currentStage.Value = 0;
            PlayerInventory.ClearWheelRewards();
        }
        
        private void HandleOnSpinWheelAnimCompleted(int slotIndex)
        {
            var slotData = _levelData.Levels[_currentStage.Value].SlotDatas[slotIndex];
            if (HasPlayerLost(slotData))
            {
                Debug.Log("Player Has Lost");
                OnGameLost?.Invoke();
                return;
            }
            
            OnRewardClaimed?.Invoke(slotData);
        }

        private void HandleOnRewardParticlesCompleted()
        {
            if (HasCompletedAllLevels())
            {
                OnGameCompleted?.Invoke();
                return;
            }
            
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
            OnShowNextStage?.Invoke(_currentStage.Value);
        }
        
        private async void HandleOnRestartButtonClicked()
        {
            await SpinWheelCloudRequests.GiveUp();
            StartGame();
        }

        private async void HandleOnReviveButtonClicked()
        {
            PlayerInventory.DecreaseCurrencyLocally(CurrencyType.Gold, GetReviveCost());
            var revivePlayerResponse = await SpinWheelCloudRequests.Revive();

            if (revivePlayerResponse.ReviveSuccessful)
            {
                _revivedAmount++;
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
            
            StartGame();
        }

        private bool HasPlayerLost(LevelSlotData slotData)
        {
            Debug.Log("Item ID : " + slotData.ID);
            return (ItemType)slotData.Type == ItemType.Bomb;
        }
        
        public int GetReviveCost()
        {
            return _reviveCost * (_revivedAmount + 1);
        }

        [ContextMenu("Increas")]
        public void TestIncreaseCurrencies()
        {
            var currencies = new Dictionary<string, float>();
            currencies.Add(PlayerInventory.GetCurrencyID(CurrencyType.Money), 10);
            currencies.Add(PlayerInventory.GetCurrencyID(CurrencyType.Gold), 5);
            EconomyCloudRequests.DecreaseCurrency(currencies);
        }

        [ContextMenu("GetCurrency")]
        public void TestGetCurrency()
        {
            EconomyCloudRequests.GetCurrency(PlayerInventory.GetCurrencyID(CurrencyType.Money));
        }
        
        private bool IsAtZoneLevel() => (LevelType)LevelData.Levels[_currentStage.Value].LevelType is LevelType.SafeZone or LevelType.SuperZone;
        private bool HasCompletedAllLevels() => _currentStage.Value == LevelData.Levels.Length - 1;
        private void AddListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted += HandleOnSpinWheelAnimCompleted;
            SpinWheelRewardsManager.OnRewardParticlesCompleted += HandleOnRewardParticlesCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent += HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent += HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked += HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked += ShowNextStage;
            WinPanelUIManager.OnPlayAgainButtonClicked += HandleOnClaimRewardsButtonClicked;
        }

        private void RemoveListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted -= HandleOnSpinWheelAnimCompleted;
            SpinWheelRewardsManager.OnRewardParticlesCompleted -= HandleOnRewardParticlesCompleted;
            LostPanelUIManager.OnRestartButtonClickedEvent -= HandleOnRestartButtonClicked;
            LostPanelUIManager.OnReviveButtonClickedEvent -= HandleOnReviveButtonClicked;
            ZonePanelUIManager.OnClaimRewardsButtonClicked -= HandleOnClaimRewardsButtonClicked;
            ZonePanelUIManager.OnContinuButtonClicked -= ShowNextStage;
            WinPanelUIManager.OnPlayAgainButtonClicked -= HandleOnClaimRewardsButtonClicked;
        }
    }
}