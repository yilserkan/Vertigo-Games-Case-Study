using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardGame.CloudServices.InventoryService
{
    public interface IInventoryCloudService
    {
        public Task<string> GetPlayerInventory();

        public Task<string> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items);
    }
}