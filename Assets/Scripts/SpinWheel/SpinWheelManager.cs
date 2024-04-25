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
        
        private WheelLevel _leveData;
        private int _currentIndex;
        private SpinWheelResponse _spinWheelResponse;
        public SpinWheelResponse SpinWheelResponse { get => _spinWheelResponse; }
        
        private SpinWheelAnimationController _spinWheelAnimationController;

        private void Awake()
        {
            ServiceLocator.For(this).Register<SpinWheelManager>(this);
        }

        private void Start()
        {
            _currentIndex = 0;
            InitializeLevel();
            ServiceLocator.For(this).Get(out _spinWheelAnimationController);
        }

        public async void InitializeLevel()
        {
            var levelJson = await SpinWheelCloudRequests.GetLevelData();
            _leveData = JsonUtility.FromJson<WheelLevel>(levelJson);
            _currentIndex = 0;
            ShowNextStage();
        }

        [ContextMenu("Next")]
        public void ShowNextStage()
        {
            InitializeSlots();
            _currentIndex++;
        }
       
        private void InitializeSlots()
        {
            var datas = _leveData.LevelData[_currentIndex];
            for (int i = 0; i < _slots.Length; i++)
            {
                if (datas.SlotDatas.Length <= i) { break; }

                if (_itemContainers.TryGetItemData((ItemType)datas.SlotDatas[i].Type, datas.SlotDatas[i].ID, out var data));
                {
                    _slots[i].InitializeSlot(data);
                }
            }
        }

        [ContextMenu("SpinWheel")]
        public async void SpinWheel()
        {
            _spinWheelAnimationController.StartSpinAnimation();
            var spinWheelResponse = await SpinWheelCloudRequests.SpinWheel();
            _spinWheelResponse = JsonUtility.FromJson<SpinWheelResponse>(spinWheelResponse);
            _spinWheelAnimationController.SetGotCloudResponse(true);
        }
    }

    [Serializable]
    public class WheelLevel
    {
        public LevelData[] LevelData;
    }
    
    [Serializable]
    public class LevelData
    {
        public LevelSlotData[] SlotDatas;
        public int LevelType;
    }

    [Serializable]
    public class LevelSlotData
    {
        public int Type;
        public string ID;
    }
}


