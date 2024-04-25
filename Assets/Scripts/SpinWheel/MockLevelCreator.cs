using CardGame.Items;
using UnityEngine;

namespace CardGame.SpinWheel
{
    public class MockLevelCreator
    {
        private ItemContainers _itemContainers;
        private int _levelCount = 50;
        
        public MockLevelCreator(ItemContainers itemContainers)
        {
            _itemContainers = itemContainers;
        }

        public LevelData[] GetMockLevelData()
        {
            var levelDatas = new LevelData[_levelCount];
            for (int i = 0; i < _levelCount; i++)
            {
                levelDatas[i] = CreateLevelData(i + 1);
            }

            return levelDatas;
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

            return slotData;
        }

        private LevelSlotData GetBombSlotData()
        {
            var itemData = _itemContainers.GetRandomBombData();
            
            var slotData = new LevelSlotData();
            slotData.Type = (int)itemData.Type;
            slotData.ID = itemData.ID;

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
        
        private enum LevelType
        {
            Default,
            SafeZone,
            SuperZone
        }
    }
}