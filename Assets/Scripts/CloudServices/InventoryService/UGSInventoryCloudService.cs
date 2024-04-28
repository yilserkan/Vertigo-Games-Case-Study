using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;

namespace CardGame.CloudServices.InventoryService
{
    public class UGSInventoryCloudService : IInventoryCloudService
    {
        private const string GET_PLAYER_INVENTORY_ENDPOINT = "GetPlayerInventory";
        private const string ADD_TO_PLAYER_INVENTORY_ENDPOINT = "AddToPlayerInventory";
        
        public async Task<string> GetPlayerInventory()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( GET_PLAYER_INVENTORY_ENDPOINT, args);
            return json;
        }

        public async Task<string> AddToPlayerInventory(IEnumerable<KeyValuePair<string, PlayerInventoryData>> items)
        {
            var args = new Dictionary<string, object>() { {"items", items}};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>( ADD_TO_PLAYER_INVENTORY_ENDPOINT, args);
            return json;
        }
    }
}