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
            var json = await _economyService.GetCurrency(currencyID);
            return JsonUtility.FromJson<GetCurrencyRespond>(json);
        }
        
        public static async Task<GetCurrenciesRespond> GetCurrencies()
        {
            var json = await _economyService.GetCurrencies();
            return JsonUtility.FromJson<GetCurrenciesRespond>(json);
        }

        public static async Task<bool> IncreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            var json = await _economyService.IncreaseCurrency(currencies);
            return JsonUtility.FromJson<bool>(json);
        }

        public static async Task<bool> DecreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            var json = await _economyService.DecreaseCurrency(currencies);
            return JsonUtility.FromJson<bool>(json);
        }
        
        public static async Task<bool> DecreaseCurrency(CurrencyType id, float amount)
        {
            var dict = new Dictionary<string, float>();
            dict.Add(PlayerInventory.GetCurrencyID(id), amount);
            var json = await _economyService.DecreaseCurrency(dict);
            return JsonUtility.FromJson<bool>(json);
        }
    }

    [Serializable]
    public class GetCurrencyRespond
    {
        public float Balance;
    }
    
    [Serializable]
    public class GetCurrenciesRespond
    {
        public CurrencyData[] CurrencyDatas;
    }

    [Serializable]
    public class CurrencyData
    {
        public string CurrencyID;
        public float Balance;
    }
}