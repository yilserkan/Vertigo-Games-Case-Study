using System.Collections.Generic;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/ItemContainers", fileName = "ItemContainers", order = 0)]
    public class ItemContainers : AbstractScriptableManagerBase
    {
        public ItemDataContainer[] Datas;
        private Dictionary<ItemType, ItemDataContainer> _datasDict;

        private ItemType[] _itemTypesWithoutBomb;
        public override void Initialize()
        {
            _itemTypesWithoutBomb = new[] { ItemType.Chest, ItemType.Costume, ItemType.Equipment, ItemType.Weapon };
            _datasDict = new Dictionary<ItemType, ItemDataContainer>();
            for (int i = 0; i < Datas.Length; i++)
            {
                if (!_datasDict.ContainsKey(Datas[i].Type))
                {
                    Datas[i].Initialize();
                    _datasDict.Add(Datas[i].Type, Datas[i]);
                }
            }
        }

        public bool TryGetItemData(ItemType type, string id, out ItemData data)
        {
            if (_datasDict.ContainsKey(type) && _datasDict[type].TryGetItemData(id, out data))
            {
                return true;
            }

            data = null;
            return false;
        }

        public ItemData GetRandomItemData()
        {
            var index = Random.Range(0, _itemTypesWithoutBomb.Length);
            var type = _itemTypesWithoutBomb[index];
            if (_datasDict.TryGetValue(type, out var container))
            {
                return container.GetRandomItemData();
            }

            return null;
        }
        
        public ItemData GetRandomBombData()
        {
            if (_datasDict.TryGetValue(ItemType.Bomb, out var container))
            {
                return container.GetRandomItemData();
            }

            return null;
        }
    }
}