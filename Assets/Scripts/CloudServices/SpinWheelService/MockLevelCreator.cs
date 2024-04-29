using CardGame.Extensions;
using CardGame.Items;
using CardGame.RemoteConfig;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using UnityEngine;

namespace CardGame.CloudServices

{
    public class MockLevelCreator
    {
        private ItemContainers _itemContainers;
        private SpinWheelConfigData _wheelConfigData;
        
        public MockLevelCreator()
        {
            ServiceLocator.Global.OrNull()?
                .Get(out _itemContainers)
                .Get(out _wheelConfigData);
        }

        public GetLevelResponse GetMockLevelData()
        {
            var levelDatas = new LevelData[_wheelConfigData.ConfigData.LevelCount];
            for (int i = 0; i < _wheelConfigData.ConfigData.LevelCount; i++)
            {
                levelDatas[i] = CreateLevelData(i + 1);
            }

            var lvl = new GetLevelResponse();
            lvl.Levels = levelDatas;
            return lvl;
        }
        
        private LevelData CreateLevelData(int level)
        {
            var levelData = new LevelData();
            levelData.SlotDatas = new LevelSlotData[_wheelConfigData.ConfigData.SliceCount];
            levelData.LevelType = (int)GetLevelType(level);

            bool containsBomb = levelData.LevelType == (int)LevelType.Default;
            var bombIndex = containsBomb ? Random.Range(0, levelData.SlotDatas.Length) : -1;
            for (int i = 0; i < levelData.SlotDatas.Length; i++)
            {
                if (containsBomb && i == bombIndex)
                {
                    levelData.SlotDatas[i] = GetBombSlotData();
                }
                else
                {
                    levelData.SlotDatas[i] = GetRandomSlotData();
                }
            }
            
            return levelData;
        }

        private LevelSlotData GetRandomSlotData()
        {
            var itemData = _itemContainers.GetRandomItemData();
            
            var slotData = new LevelSlotData();
            slotData.Type = (int)itemData.Type;
            slotData.ID = itemData.ID;
            slotData.Amount = Random.Range(0, 10);

            return slotData;
        }

        private LevelSlotData GetBombSlotData()
        {
            var itemData = _itemContainers.GetRandomBombData();
            
            var slotData = new LevelSlotData();
            slotData.Type = (int)itemData.Type;
            slotData.ID = itemData.ID;
            slotData.Amount = Random.Range(0, 10);
            
            return slotData;
        }

        private LevelType GetLevelType(int level)
        {
            if (level == 1 )
            {
                return LevelType.Default;
            }
            
            if (level % _wheelConfigData.ConfigData.SuperZoneInterval == 0)
            {
                return LevelType.SuperZone;
            }
            if (level % _wheelConfigData.ConfigData.SafeZoneInterval == 0)
            {
                return LevelType.SafeZone;
            }

            return LevelType.Default;
        }
    }
}