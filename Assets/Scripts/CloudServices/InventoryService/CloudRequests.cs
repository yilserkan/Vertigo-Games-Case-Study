using System;
using System.Threading.Tasks;

namespace CardGame.CloudServices
{
    public static class CloudRequests
    {
        // Unity Services are not reachable sometimes so wrote this func to try multiple times
        public static async Task<T> RequestRepeated<T>(Task<T> cloudRequest, int amount = 3) where T : BaseResponse
        {
            var datas = await cloudRequest;
            
            int triedCount = 1;
            while (!datas.RequestSuccessful)
            {
                if (triedCount >= amount) { break; }
                triedCount++;
                datas = await cloudRequest;
                await Task.Delay(500);
              
            }
            
            return datas;
        }
    }
    
    [Serializable]
    public class BaseResponse
    {
        public bool RequestSuccessful;
    }
}