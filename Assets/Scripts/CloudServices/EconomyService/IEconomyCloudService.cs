using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardGame.CloudServices.EconomyService
{
    public interface IEconomyCloudService
    {
        public Task<string> GetCurrency(string currencyID);
        public Task<string> GetCurrencies();
        public Task<string> IncreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies);
        public Task<string> DecreaseCurrency(IEnumerable<KeyValuePair<string, float>> currencies);
    }
}