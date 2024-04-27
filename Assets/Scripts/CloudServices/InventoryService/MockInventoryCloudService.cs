using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Inventory;
using UnityEngine;

namespace CardGame.CloudServices.InventoryService
{
    public class MockInventoryCloudService : IInventoryCloudService
    {
        private const string INVENTORY_PREF_KEY = "INVENTORY_ITEMS_PREF_KEY";
        
        public Task<string> GetPlayerInventory()
        {
            var json = PlayerPrefs.GetString(INVENTORY_PREF_KEY, "{}");
            return Task.FromResult(json);
        }

        public Task<string> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items)
        {
            foreach (var inventoyItem in items)
            {
                PlayerInventory.AddToInventoryItems(inventoyItem.Value);
            }
            
            SaveInventory();
            return Task.FromResult(JsonUtility.ToJson(true));
        }

        private void SaveInventory()
        {
            var inventory = PlayerInventory.InventoryItemDatas;

            var inventoryPrefData = new GetPlayerInventoryResponse();
            inventoryPrefData.InventoryDatas = new PlayerInventoryData[inventory.Count];
            int i = 0;
            foreach (var inventoryItem in inventory)
            {
                inventoryPrefData.InventoryDatas[i] = new PlayerInventoryData()
                    { ID = inventoryItem.Key, Amount = inventoryItem.Value.Amount };
                i++;
            }

            var json = JsonUtility.ToJson(inventoryPrefData);
            PlayerPrefs.SetString(INVENTORY_PREF_KEY, json);
        }
    }
}