using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.CloudServices.EconomyService;
using CardGame.CloudServices.InventoryService;
using CardGame.Extensions;
using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.Inventory
{
    public static class PlayerInventory
    {
        private static Dictionary<string, InventoryItemData> _inventoryItems = new Dictionary<string, InventoryItemData>();
        private static Dictionary<string, PlayerInventoryData> _wheelRewards = new Dictionary<string, PlayerInventoryData>();
        private static Dictionary<string, Observable<float>> _currencies = new Dictionary<string, Observable<float>>();
        public static Dictionary<string, InventoryItemData> InventoryItemDatas => _inventoryItems;
        public static Dictionary<string, PlayerInventoryData> WheelRewards => _wheelRewards;
        public static Dictionary<string, Observable<float>> Currencies = _currencies;
        
        public static async Task Initialize()
        {
            await InitializeCurrencies();
            await InitializeInventoryItems();
        }

        private static async Task InitializeInventoryItems()
        {
            var datas = await CloudRequests.RequestRepeated(InventoryCloudRequests.GetPlayerInventory(), 3);
            if (datas == null || datas.InventoryDatas == null) { return; }
            
            for (int i = 0; i < datas.InventoryDatas.Length; i++)
            {
                if (!_inventoryItems.ContainsKey(datas.InventoryDatas[i].ID))
                {
                    var itemData = new InventoryItemData() { Amount = datas.InventoryDatas[i].Amount, Type = datas.InventoryDatas[i].Type};
                    _inventoryItems.Add(datas.InventoryDatas[i].ID, itemData);
                }
            }
        }

        private static async Task InitializeCurrencies()
        {
            var currencies = await CloudRequests.RequestRepeated(EconomyCloudRequests.GetCurrencies(), 3);
            if (currencies == null) { return; }

            for (int i = 0; i < currencies.CurrencyDatas.Length; i++)
            {
                var currency = currencies.CurrencyDatas[i];
                if (!_currencies.ContainsKey(currency.CurrencyID))
                {
                    _currencies.Add(currency.CurrencyID, new Observable<float>(currency.Balance));
                }
            }
        }
        
        public static void AddToInventoryItems(PlayerInventoryData data)
        {
            if (_inventoryItems.ContainsKey(data.ID))
            {
                _inventoryItems[data.ID].Amount += data.Amount;
            }
            else
            {
                InventoryItemDatas[data.ID] = new InventoryItemData() { Amount = data.Amount, Type = data.Type};
            }
        }

        public static void ClaimRewardsLocally()
        {
            if (_wheelRewards == null) { return; }

            foreach (var reward in _wheelRewards)
            {
                if (reward.Value.Type == (int)ItemType.Currency)
                {
                    if (_currencies.ContainsKey(reward.Value.ID))
                    {
                        _currencies[reward.Value.ID].Value += reward.Value.Amount;
                    }
                    else
                    {
                        _currencies.Add(reward.Value.ID, new Observable<float>(reward.Value.Amount));
                    }
                }
                else
                {
                    if (_inventoryItems.ContainsKey(reward.Value.ID))
                    {
                        _inventoryItems[reward.Value.ID].Amount += reward.Value.Amount;
                    }
                    else
                    {
                        _inventoryItems.Add(reward.Value.ID, new InventoryItemData(){Type = reward.Value.Type, Amount = reward.Value.Amount});
                    }
                }
            }
        }
        
        public static void AddToWheelRewards(PlayerInventoryData slotData)
        {
            if (_wheelRewards.ContainsKey(slotData.ID))
            {
                _wheelRewards[slotData.ID].Amount += slotData.Amount;
            }
            else
            {
                _wheelRewards.Add(slotData.ID, slotData);
            }
        }

        public static void ClearWheelRewards()
        {
            _wheelRewards = new Dictionary<string, PlayerInventoryData>();
        }

        public static string GetCurrencyID(CurrencyType currencyType)
        {
            ItemContainers itemContainers = null;
            ServiceLocator.Global.OrNull()?.Get(out itemContainers);
            if (itemContainers != null && itemContainers.TryGetCurrencyItem(currencyType, out var item))
            {
                return item.ID;
            }

            return "";
        }

        public static float GetCurrencyAmount(CurrencyType currencyType)
        {
            var currencyKey = GetCurrencyID(currencyType);
            if (_currencies.ContainsKey(currencyKey))
            {
                return _currencies[currencyKey].Value;
            }

            return 0;
        }

        public static void DecreaseCurrencyLocally(CurrencyType currencyType, float amount)
        {
            var currencyKey = GetCurrencyID(currencyType);
            if (_currencies.ContainsKey(currencyKey))
            {
                var decreasedAmount = Mathf.Max(0, _currencies[currencyKey].Value - amount);
                
                _currencies[currencyKey].Value = decreasedAmount;
            }
        }
    }
    
    public class InventoryItemData
    {
        public float Amount;
        public int Type;
    }
}




