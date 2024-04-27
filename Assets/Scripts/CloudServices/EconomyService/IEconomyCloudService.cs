using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardGame.CloudServices.EconomyService
{
    public interface IEconomyCloudService
    {
        public Task<string> GetCurrencies();
        public Task<string> IncrementCurrency(IEnumerable<KeyValuePair<string, float>> currencies);
    }
}