using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/ItemDataContainer", fileName = "ItemDataContainer", order = 0)]
    public class ItemDataContainer : ScriptableObject
    {
        public ItemType Type;
        public ItemData[] Datas;
        private Dictionary<string, ItemData> _datasDict;

        public void Initialize()
        {
            _datasDict = new Dictionary<string, ItemData>();
            for (int i = 0; i < Datas.Length; i++)
            {
                if (!_datasDict.ContainsKey(Datas[i].ID))
                {
                    _datasDict.Add(Datas[i].ID, Datas[i]);
                }        
            }
        }

        public bool TryGetItemData(string id, out ItemData data)
        {
            if (_datasDict.TryGetValue(id, out var value))
            {
                data = value;
                return true;
            }

            data = null;
            return false;
        }

        public ItemData GetRandomItemData()
        {
            var randIndex = Random.Range(0, Datas.Length);
            return _datasDict.GetValueOrDefault(Datas[randIndex].ID);
        }
    }
}
