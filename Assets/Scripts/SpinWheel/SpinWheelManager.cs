using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Items;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.SpinWheel
{
    public class SpinWheelManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelSlotManager[] _slots;
        [SerializeField] private ItemContainers _itemContainers;

        private LevelManager _levelManager;
        private SpinWheelResponse _spinWheelResponse;
        private SpinWheelAnimationController _spinWheelAnimationController;
        private SpinWheelUIManager _spinWheelUIManager;
        
        public static event Action<int> OnSpinAnimationCompleted;

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
            ServiceLocator.For(this).Register<SpinWheelManager>(this);
        }

        private void Start()
        {
            ServiceLocator.For(this)
                .Get(out _spinWheelAnimationController)
                .Get(out _spinWheelUIManager)
                .Get(out _levelManager);
        }
        
        private void StartGame()
        {
            ShowStage(_levelManager.CurrentStage);
        }
        
        public void ShowStage(int stage)
        {
            InitializeSlots(stage);
        }
       
        private void InitializeSlots(int stage)
        {
            var datas = _levelManager.LevelData.Levels[stage];
            for (int i = 0; i < _slots.Length; i++)
            {
                if (datas.SlotDatas.Length <= i) { break; }

                if (_itemContainers.TryGetItemData((ItemType)datas.SlotDatas[i].Type, datas.SlotDatas[i].ID, out var data));
                {
                    _slots[i].InitializeSlot(data, datas.SlotDatas[i].Amount);
                }
                _spinWheelUIManager.SetWheelUI((LevelType)datas.LevelType);
            }
        }

        [ContextMenu("SpinWheel")]
        public async void SpinWheel()
        {
            _spinWheelAnimationController.StartSpinAnimation();
            _spinWheelResponse = await SpinWheelCloudRequests.SpinWheel();
            _spinWheelAnimationController.SetTargetSlotIndex(_spinWheelResponse.SlotIndex);
        }

        public void OnSpinWheelAnimationCompleted()
        {
            _spinWheelUIManager.SetSpinButtonInteractable(true);
            OnSpinAnimationCompleted?.Invoke(_spinWheelResponse.SlotIndex);
        }

        public int GetSlotCount() => _slots.Length;
        
        private void AddListeners()
        {
            LevelManager.OnStartGame += StartGame;
            LevelManager.OnShowNextStage += ShowStage;
        }
        private void RemoveListeners()
        {
            LevelManager.OnStartGame -= StartGame;
            LevelManager.OnShowNextStage -= ShowStage;
        }
    }
    
    public enum LevelType
    {
        Default,
        SafeZone,
        SuperZone
    }
}


