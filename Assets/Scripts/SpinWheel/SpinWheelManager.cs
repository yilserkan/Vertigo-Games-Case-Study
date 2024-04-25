using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Items;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.SpinWheel
{
    public class SpinWheelManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelSlotManager[] _slots;
        [SerializeField] private ItemData[] _datas;
        [SerializeField] private ItemContainers _itemContainers;

        private MockLevelCreator _mockLevelCreator;
        private LevelData[] _leveData;
        private int _currentIndex;
        
        private void Start()
        {
            _mockLevelCreator = new MockLevelCreator(_itemContainers);
            _leveData = _mockLevelCreator.GetMockLevelData();
            _currentIndex = 0;
            InitializeSlots();
        }

        [ContextMenu("Next")]
        private void InitializeSlots()
        {
            var datas = _leveData[_currentIndex];
            for (int i = 0; i < _slots.Length; i++)
            {
                if (datas.SlotDatas.Length <= i) { break; }

                if (_itemContainers.TryGetItemData((ItemType)datas.SlotDatas[i].Type, datas.SlotDatas[i].ID, out var data));
                {
                    _slots[i].InitializeSlot(data);
                }
            }

            _currentIndex++;
        }
    }

    public class LevelData
    {
        public LevelSlotData[] SlotDatas;
        public int LevelType;
    }

    public class LevelSlotData
    {
        public int Type;
        public string ID;
    }
}


