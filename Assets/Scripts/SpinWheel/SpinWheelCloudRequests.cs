using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.CloudServices.EconomyService;
using CardGame.CloudServices.InventoryService;
using CardGame.Inventory;
using CardGame.Items;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public static class SpinWheelCloudRequests
    {
        private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        
        public static async Task<GetLevelResponse> GetLevelData()
        {
            var json = await _cloudService.GetLevelData();
            return JsonUtility.FromJson<GetLevelResponse>(json);
        }

        public static async Task<SpinWheelResponse> SpinWheel()
        {
            var json = await _cloudService.SpinWheel();
            return JsonUtility.FromJson<SpinWheelResponse>(json);
        }

        public static async Task<RevivePlayerResponse> Revive()
        {
            var json = await _cloudService.Revive();
            return JsonUtility.FromJson<RevivePlayerResponse>(json);
        }

        public static async Task GiveUp()
        {
            await _cloudService.GiveUp();
        }

        public static async Task ClaimRewards()
        {
            await _cloudService.ClaimRewards();
        }
    }

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
            //TODO:
            // Set player state from death to alive   
            // Decrease Player Money

            var respond = await EconomyCloudRequests.GetCurrency(PlayerInventory.GetCurrencyID(CurrencyType.Gold));
            var hasPlayerEnoughMoney = respond.Balance >= LostPanelUIManager.REVIVE_COST;
            await EconomyCloudRequests.DecreaseCurrency(CurrencyType.Gold,LostPanelUIManager.REVIVE_COST);
            var response = new RevivePlayerResponse() { ReviveSuccessful = hasPlayerEnoughMoney };
            return JsonUtility.ToJson(response);
        }

        public Task GiveUp()
        {
            PlayerInventory.ClearWheelRewards();
            return Task.CompletedTask;
        }

        public async Task ClaimRewards()
        {
            var inventoryItemRewards =
                PlayerInventory.WheelRewards.Where(kv => kv.Value.Type != (int)ItemType.Currency); 
            await InventoryCloudRequests.AddToPlayerInventory(ConvertDictionaryForInventoryRequest(inventoryItemRewards));
            
            var currencyRewards =
                PlayerInventory.WheelRewards.Where(kv => kv.Value.Type == (int)ItemType.Currency);
            await EconomyCloudRequests.IncreaseCurrency(ConvertDictionaryForCurrencyRequest(currencyRewards));
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

    public interface ISpinWheelCloudService
    {
        public Task<string> GetLevelData();
        public Task<string> SpinWheel();

        public Task<string> Revive();
        public Task GiveUp();
        public Task ClaimRewards();
    }

    [Serializable]
    public class RevivePlayerResponse
    {
        [FormerlySerializedAs("ReviveSuccessfull")] public bool ReviveSuccessful;
    }
    
    [Serializable]
    public class GetLevelResponse
    {
        public LevelData[] Levels;
    }

    [Serializable]
    public class LevelData
    {
        public LevelSlotData[] SlotDatas;
        public int LevelType;
    }

    [Serializable]
    public class LevelSlotData
    {
        public int Type;
        public string ID;
        public int Amount;
    }
    
    [Serializable]
    public class SpinWheelResponse
    {
        public int SlotIndex;
    }

}