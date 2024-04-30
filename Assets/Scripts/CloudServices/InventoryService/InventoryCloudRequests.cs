using System;
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
            try
            {
                var json = await _cloudService.GetPlayerInventory();
                var response = JsonUtility.FromJson<GetPlayerInventoryResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GetPlayerInventoryResponse() { RequestSuccessful = false , InventoryDatas = Array.Empty<PlayerInventoryData>() };
                return response;
            }
        }
        
        public static async Task<AddToInventoryRespond> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items)
        {
            try
            {
                var json = await _cloudService.AddToPlayerInventory(items);
                var response = JsonUtility.FromJson<AddToInventoryRespond>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new AddToInventoryRespond(){RequestSuccessful = false, Successful = false};
                return response;
            }
        }
    }
    
    [Serializable]
    public class GetPlayerInventoryResponse : BaseResponse
    {
        public PlayerInventoryData[] InventoryDatas;
    }
    
    [Serializable]
    public class AddToInventoryRespond : BaseResponse
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

