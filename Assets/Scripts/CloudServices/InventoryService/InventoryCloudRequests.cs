using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.CloudServices.InventoryService
{
    public static class InventoryCloudRequests
    {
        // private static IInventoryCloudService _cloudService = new MockInventoryCloudService();
        private static IInventoryCloudService _cloudService = new UGSInventoryCloudService();

        public static async Task<GetPlayerInventoryResponse> GetPlayerInventory()
        {
            var json = await _cloudService.GetPlayerInventory();
            return JsonUtility.FromJson<GetPlayerInventoryResponse>(json);
        }
        
        public static async Task<AddToInventoryRespond> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items)
        {
             var json = await _cloudService.AddToPlayerInventory(items);
             return JsonUtility.FromJson<AddToInventoryRespond>(json);
        }
        
    }

    [Serializable]
    public class GetPlayerInventoryResponse
    {
        public PlayerInventoryData[] InventoryDatas;
    }
    
    [Serializable]
    public class AddToInventoryRespond
    {
        public bool Successful;
    }

    [Serializable]
    public class PlayerInventoryData
    {
        public string ID;
        public float Amount;
        public int Type;
    }
}

