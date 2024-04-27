using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Inventory;
using CardGame.Items;
using CardGame.ServiceManagement;
using UnityEngine;

namespace CardGame.CloudServices.EconomyService
{
    public class MockEconomyCloudService : IEconomyCloudService
    {
        private const string PLAYER_PREF_CURRENCY_PREFIX = "PREF_CURRENY_";
        
        public Task<string> GetCurrencies()
        {
            ServiceLocator.Global.Get(out ItemContainers itemContainers);
            if (itemContainers == null) { return Task.FromResult(JsonUtility.ToJson("{}")); }
            var currencyDataList = new List<CurrencyData>();
            if (itemContainers.TryGetCurrencyItem(CurrencyType.Gold, out var goldItem))
            {
                var gold = PlayerPrefs.GetFloat($"{PLAYER_PREF_CURRENCY_PREFIX}{goldItem.ID}", 0);
                currencyDataList.Add(new CurrencyData(){CurrencyID = goldItem.ID, Balance = gold});
            }
            if (itemContainers.TryGetCurrencyItem(CurrencyType.Money, out var moneyItem))
            {
                var money = PlayerPrefs.GetFloat($"{PLAYER_PREF_CURRENCY_PREFIX}{moneyItem.ID}", 0);
                currencyDataList.Add(new CurrencyData(){CurrencyID = moneyItem.ID, Balance = money});
            }

            var response = new GetCurrencyRespond()
            {
                CurrencyDatas = currencyDataList.ToArray()
            };
            
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> IncrementCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            foreach (var currency in currencies)
            {
                var currentAmount = PlayerPrefs.GetFloat($"{PLAYER_PREF_CURRENCY_PREFIX}{currency.Key}", 0);
                PlayerPrefs.SetFloat($"{PLAYER_PREF_CURRENCY_PREFIX}{currency.Key}", currentAmount + currency.Value);
            }
            
            return Task.FromResult(JsonUtility.ToJson(true));
        }
    }
}