using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;

namespace CardGame.CloudServices
{
    public class UGSSpinWheelCloudService : ISpinWheelCloudService
    {
        private const string GET_WHEEL_LEVEL_DATA_ENDPOINT = "GetWheelLevelData";
        private const string SPIN_WHEEL_ENDPOINT = "SpinWheel";
        private const string REVIVE_ENDPOINT = "Revive";
        private const string GIVE_UP_ENDPOINT = "GiveUp";
        private const string CLAIM_REWARDS_ENDPOINT = "ClaimRewards";
        
        public async Task<string> GetLevelData()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>(GET_WHEEL_LEVEL_DATA_ENDPOINT, args);
            return json;
        }

        public async Task<string> SpinWheel()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>(SPIN_WHEEL_ENDPOINT, args);
            return json;
        }

        public async Task<string> Revive()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>(REVIVE_ENDPOINT, args);
            return json;
        }

        public async Task<string> GiveUp()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>(GIVE_UP_ENDPOINT, args);
            return json;
        }

        public async Task<string> ClaimRewards()
        {
            var args = new Dictionary<string, object>() {};
            var json = await CloudCodeService.Instance.CallEndpointAsync<string>(CLAIM_REWARDS_ENDPOINT, args);
            return json;
        }
    }
}