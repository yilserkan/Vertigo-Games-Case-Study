using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using UnityEngine;

namespace CardGame.CloudServices

{
    public class MockLevelCreator
    {
        private ItemContainers _itemContainers;
        private int _levelCount = 60;
        
        public MockLevelCreator()
        {
            ServiceLocator.LazyGlobal.Get(out _itemContainers);
        }

        public GetLevelResponse GetMockLevelData()
        {
            var levelDatas = new LevelData[_levelCount];
            for (int i = 0; i < _levelCount; i++)
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
            levelData.SlotDatas = new LevelSlotData[8];
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
            
            if (level % 30 == 0)
            {
                return LevelType.SuperZone;
            }
            if (level % 5 == 0)
            {
                return LevelType.SafeZone;
            }

            return LevelType.Default;
        }
    }
}