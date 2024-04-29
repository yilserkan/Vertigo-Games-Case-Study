using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.CloudServices.EconomyService;
using CardGame.CloudServices.InventoryService;
using CardGame.Extensions;
using CardGame.Inventory;
using CardGame.Items;
using CardGame.ServiceManagement;
using CardGame.SpinWheel;
using UnityEngine;

namespace CardGame.CloudServices
{
    public class MockSpinWheelCloudService : ISpinWheelCloudService
    {
        private static MockLevelCreator _levelCreator = new();
        
        
        public Task<string> GetLevelData()
        {
            return Task.FromResult(JsonUtility.ToJson(_levelCreator.GetMockLevelData()));
        }

        public Task<string> SpinWheel()
        {
            var randIndex = Random.Range(0, 8);
            var response = new SpinWheelResponse(){SlotIndex = randIndex};
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public async Task<string> Revive()
        {
            int reviveCost = 0;
            LevelManager levelManager = null;
            ServiceLocator.ForActiveScene()?.Get(out levelManager);
            if (levelManager != null)
            {
                reviveCost = levelManager.GetReviveCost();
            }
            
            var respond = await EconomyCloudRequests.GetCurrency(PlayerInventory.GetCurrencyID(CurrencyType.Gold));
            var hasPlayerEnoughMoney = respond.Balance >= reviveCost;
            await EconomyCloudRequests.DecreaseCurrency(CurrencyType.Gold,reviveCost);
            var response = new RevivePlayerResponse() { ReviveSuccessful = hasPlayerEnoughMoney };
            return JsonUtility.ToJson(response);
        }

        public Task<string> GiveUp()
        {
            PlayerInventory.ClearWheelRewards();
            var response = new GiveUpResponse() { Successful = true };
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public async Task<string> ClaimRewards()
        {
            var inventoryItemRewards =
                PlayerInventory.WheelRewards.Where(kv => kv.Value.Type != (int)ItemType.Currency); 
            await InventoryCloudRequests.AddToPlayerInventory(ConvertDictionaryForInventoryRequest(inventoryItemRewards));
            
            var currencyRewards =
                PlayerInventory.WheelRewards.Where(kv => kv.Value.Type == (int)ItemType.Currency);
            await EconomyCloudRequests.IncreaseCurrency(ConvertDictionaryForCurrencyRequest(currencyRewards));

            var response = new ClaimRewardsResponse() { Successful = true };
            return JsonUtility.ToJson(response);
        }

        private Dictionary<string, PlayerInventoryData> ConvertDictionaryForInventoryRequest(IEnumerable<KeyValuePair<string, PlayerInventoryData>> datas)
        {
            var newDict = new Dictionary<string, PlayerInventoryData>();
            foreach (var kv in datas)
            {
                if (!newDict.ContainsKey(kv.Key))
                {
                    newDict.Add(kv.Key, kv.Value);
                }
            }

            return newDict;
        }
        
        private Dictionary<string, float> ConvertDictionaryForCurrencyRequest(IEnumerable<KeyValuePair<string, PlayerInventoryData>> datas)
        {
            var newDict = new Dictionary<string, float>();
            foreach (var kv in datas)
            {
                if (!newDict.ContainsKey(kv.Key))
                {
                    newDict.Add(kv.Key, kv.Value.Amount);
                }
            }

            return newDict;
        }
    }
}