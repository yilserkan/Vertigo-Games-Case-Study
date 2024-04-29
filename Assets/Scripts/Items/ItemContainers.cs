using System;
using System.Collections.Generic;
using CardGame.Extensions;
using CardGame.ServiceManagement;
using CardGame.Utils;
using Unity.Services.RemoteConfig;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/ItemContainers", fileName = "ItemContainers", order = 0)]
    public class ItemContainers : AbstractScriptableManagerBase
    {
        public ItemDataContainer[] Datas;
        private Dictionary<ItemType, ItemDataContainer> _datasDict;

        private List<ItemType> _itemTypesWithoutBomb;
        public override void Initialize()
        {
            base.Initialize();
            ServiceLocator.LazyGlobal.OrNull()?.Register(this);
            _itemTypesWithoutBomb = new List<ItemType>();
            _datasDict = new Dictionary<ItemType, ItemDataContainer>();
            
            for (int i = 0; i < Datas.Length; i++)
            {
                if (!_datasDict.ContainsKey(Datas[i].Type))
                {
                    Datas[i].Initialize();
                    _datasDict.Add(Datas[i].Type, Datas[i]);
                }

                if (Datas[i].Type != ItemType.Bomb && !_itemTypesWithoutBomb.Contains(Datas[i].Type))
                {
                    _itemTypesWithoutBomb.Add(Datas[i].Type);
                }
            }
                        
            var items = RemoteConfigService.Instance.appConfig.GetJson("ITEMS");
            var remoteConfigItems = JsonUtility.FromJson<RemoteConfigItems>(items);
            for (int i = 0; i < remoteConfigItems.Items.Length; i++)
            {
                var item = remoteConfigItems.Items[i];
                var type = (ItemType)item.Type;
                if (_datasDict.ContainsKey(type))
                {
                    _datasDict[type].SetRemoteConfigData(item);
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
            var index = Random.Range(0, _itemTypesWithoutBomb.Count);
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

        public bool TryGetCurrencyItem(CurrencyType currencyType, out ItemData currencyItemData)
        {
            if (_datasDict.ContainsKey(ItemType.Currency))
            {
                for (int i = 0; i <  _datasDict[ItemType.Currency].Datas.Length; i++)
                {
                    var data = _datasDict[ItemType.Currency].Datas[i] as CurrencyItemData;
                    if (data != null && data.CurrencyType == currencyType)
                    {
                        currencyItemData = data;
                        return true;
                    }
                }
            }

            currencyItemData = null;
            return false;
        }
        
        public bool TryGetCurrencyItem(string id, out ItemData currencyItemData)
        {
            if (_datasDict.ContainsKey(ItemType.Currency))
            {
                for (int i = 0; i <  _datasDict[ItemType.Currency].Datas.Length; i++)
                {
                    var data = _datasDict[ItemType.Currency].Datas[i] as CurrencyItemData;
                    if (data != null && data.ID == id)
                    {
                        currencyItemData = data;
                        return true;
                    }
                }
            }

            currencyItemData = null;
            return false;
        }
    }
    
    [Serializable]
    public class RemoteConfigItems
    {
        public RemoteConfigItemData[] Items;
    }
    
    [Serializable]
    public class RemoteConfigItemData
    {
        public string ID;
        public string Name;
        public int Type;
        public int Rarity;
    }
}