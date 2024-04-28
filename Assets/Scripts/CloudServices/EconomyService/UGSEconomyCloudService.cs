using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;

namespace CardGame.CloudServices.EconomyService
{
    public class UGSEconomyCloudService : IEconomyCloudService
    {
        private const string GET_CURRENCY_ENPOINT = "GetCurrency";
        private const string GET_CURRENCIES_ENPOINT = "GetCurrencies";
        private const string INCREASE_CURRENCY_ENDPOINT = "IncreaseCurrencies";
        private const string DECREASE_CURRENCY_ENDPOINT = "DecreaseCurrencies";
        
        public async Task<string> GetCurrency(string currencyID)
        {
            var args = new Dictionary<string, object>() { {"currencyId", currencyID} };
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( GET_CURRENCY_ENPOINT, args);
            return json;
        }

        public async Task<string> GetCurrencies()
        {
            var args = new Dictionary<string, object>() { };
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( GET_CURRENCIES_ENPOINT, args);
            return json;
        }

        public async Task<string> IncreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            var args = new Dictionary<string, object>() { {"currencies", currencies}};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( INCREASE_CURRENCY_ENDPOINT, args);
            return json;
        }

        public async Task<string> DecreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies)
        {
            var args = new Dictionary<string, object>() { {"currencies", currencies}};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( DECREASE_CURRENCY_ENDPOINT, args);
            return json;
        }
    }
}