using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Inventory;
using CardGame.Items;
using UnityEngine;

namespace CardGame.CloudServices.EconomyService
{
    public static class EconomyCloudRequests
    {
        // private static IEconomyCloudService _economyService = new MockEconomyCloudService();
        private static IEconomyCloudService _economyService = new UGSEconomyCloudService();

        public static async Task<GetCurrencyRespond> GetCurrency(string currencyID)
        {
            try
            {
                var json = await _economyService.GetCurrency(currencyID);
                var response = JsonUtility.FromJson<GetCurrencyRespond>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GetCurrencyRespond() { RequestSuccessful = false, Balance = 0 };
                return response;
            }
          
        }
        
        public static async Task<GetCurrenciesRespond> GetCurrencies()
        {
            try
            {
                var json = await _economyService.GetCurrencies();
                var response = JsonUtility.FromJson<GetCurrenciesRespond>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GetCurrenciesRespond() { RequestSuccessful = false, CurrencyDatas = Array.Empty<CurrencyData>()};
                return response;
            }
        }

        public static async Task<UpdateCurrencyResponse> IncreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            try
            {
                var json = await _economyService.IncreaseCurrency(currencies);
                var response = JsonUtility.FromJson<UpdateCurrencyResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new UpdateCurrencyResponse() { RequestSuccessful = false, Successful = false};
                return response;
            }
        }

        public static async Task<UpdateCurrencyResponse> DecreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            try
            {
                var json = await _economyService.DecreaseCurrency(currencies);
                var response = JsonUtility.FromJson<UpdateCurrencyResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new UpdateCurrencyResponse() { RequestSuccessful = false, Successful = false};
                return response;
            }
        }
        
        public static async Task<UpdateCurrencyResponse> DecreaseCurrency(CurrencyType id, float amount)
        {
            var dict = new Dictionary<string, float>();
            dict.Add(PlayerInventory.GetCurrencyID(id), amount);
            var data = await DecreaseCurrency(dict);
            return data;
        }
    }

    [Serializable]
    public class GetCurrencyRespond : BaseResponse
    {
        public float Balance;
    }

    [Serializable]
    public class UpdateCurrencyResponse : BaseResponse
    {
        public bool Successful;
    }
    
    [Serializable]
    public class GetCurrenciesRespond : BaseResponse
    {
        public CurrencyData[] CurrencyDatas;
    }

    [Serializable]
    public class CurrencyData : BaseResponse
    {
        public string CurrencyID;
        public float Balance;
    }
}