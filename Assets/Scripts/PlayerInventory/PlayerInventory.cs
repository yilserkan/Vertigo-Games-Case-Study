using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.CloudServices.EconomyService;
using CardGame.CloudServices.InventoryService;
using CardGame.SpinWheel;
using UnityEngine;

namespace CardGame.Inventory
{
    public static class PlayerInventory
    {
        private static Dictionary<string, InventoryItemData> _inventoryItems = new Dictionary<string, InventoryItemData>();
        private static Dictionary<string, PlayerInventoryData> _wheelRewards = new Dictionary<string, PlayerInventoryData>();
        private static Dictionary<string, float> _currencies = new Dictionary<string, float>();
        public static Dictionary<string, InventoryItemData> InventoryItemDatas => _inventoryItems;
        public static Dictionary<string, PlayerInventoryData> WheelRewards => _wheelRewards;
        public static Dictionary<string, float> Currencies = _currencies;
        
        public static async Task Initialize()
        {
            await InitializeInventoryItems();
            await InitializeCurrencies();
        }

        private static async Task InitializeInventoryItems()
        {
            var datas = await InventoryCloudRequests.GetPlayerInventory();
            if (datas == null) { return; }
            
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
            var currencies = await EconomyCloudRequests.GetCurrencies();
            if (currencies == null) { return; }

            for (int i = 0; i < currencies.CurrencyDatas.Length; i++)
            {
                var currency = currencies.CurrencyDatas[i];
                if (!_currencies.ContainsKey(currency.CurrencyID))
                {
                    _currencies.Add(currency.CurrencyID, currency.Balance);
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
    
        public static void AddToWheelRewards(PlayerInventoryData slotData)
        {
            _wheelRewards[slotData.ID] = slotData;
        }

        public static void ClearWheelRewards()
        {
            _wheelRewards = new Dictionary<string, PlayerInventoryData>();
        }
    }
    
    public class InventoryItemData
    {
        public float Amount;
        public int Type;
    }
}




