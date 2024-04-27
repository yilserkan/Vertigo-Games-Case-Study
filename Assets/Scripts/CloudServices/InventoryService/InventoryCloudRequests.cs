using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.CloudServices.InventoryService
{
    public static class InventoryCloudRequests
    {
        private static IInventoryCloudService _cloudService = new MockInventoryCloudService();

        public static async Task<GetPlayerInventoryResponse> GetPlayerInventory()
        {
            var json = await _cloudService.GetPlayerInventory();
            return JsonUtility.FromJson<GetPlayerInventoryResponse>(json);
        }
        
        public static async Task<bool> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items)
        {
             var json = await _cloudService.AddToPlayerInventory(items);
             return JsonUtility.FromJson<bool>(json);
        }
        
    }

    [Serializable]
    public class GetPlayerInventoryResponse
    {
        public PlayerInventoryData[] InventoryDatas;
    }

    [Serializable]
    public class PlayerInventoryData
    {
        public string ID;
        public float Amount;
        public int Type;
    }
    
}

