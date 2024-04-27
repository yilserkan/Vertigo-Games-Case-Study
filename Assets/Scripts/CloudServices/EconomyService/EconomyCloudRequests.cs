using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CardGame.CloudServices.EconomyService
{
    public static class EconomyCloudRequests
    {
        private static IEconomyCloudService _economyService = new MockEconomyCloudService();
        
        public static async Task<GetCurrencyRespond> GetCurrencies()
        {
            var json = await _economyService.GetCurrencies();
            return JsonUtility.FromJson<GetCurrencyRespond>(json);
        }

        public static async Task<bool> IncreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            var json = await _economyService.IncrementCurrency(currencies);
            return JsonUtility.FromJson<bool>(json);
        }
    }

    [Serializable]
    public class GetCurrencyRespond
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