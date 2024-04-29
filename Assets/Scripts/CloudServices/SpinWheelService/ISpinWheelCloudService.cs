using System.Threading.Tasks;

namespace CardGame.CloudServices
{
    public interface ISpinWheelCloudService
    {
        public Task<string> GetLevelData();
        public Task<string> SpinWheel();
        public Task<string> Revive();
        public Task<string> GiveUp();
        public Task<string> ClaimRewards();
    }
}