using System;
using System.Threading.Tasks;
using UnityEngine;


namespace CardGame.CloudServices
{
    public static class SpinWheelCloudRequests
    {
        // private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        private static ISpinWheelCloudService _cloudService = new UGSSpinWheelCloudService();
        
        public static async Task<GetLevelResponse> GetLevelData()
        {
            try
            {
                var json = await _cloudService.GetLevelData();
                var response = JsonUtility.FromJson<GetLevelResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GetLevelResponse() { RequestSuccessful = false, Levels = Array.Empty<LevelData>()};
                return response;
            }
        }

        public static async Task<SpinWheelResponse> SpinWheel()
        {
            try
            {
                var json = await _cloudService.SpinWheel();
                var response = JsonUtility.FromJson<SpinWheelResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new SpinWheelResponse() { RequestSuccessful = false, SlotIndex = 0};
                return response;
            }
        }

        public static async Task<RevivePlayerResponse> Revive()
        {
            try
            {
                var json = await _cloudService.Revive();
                var response = JsonUtility.FromJson<RevivePlayerResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new RevivePlayerResponse() { RequestSuccessful = false, ReviveSuccessful = false};
                return response;
            }
        }

        public static async Task<GiveUpResponse> GiveUp()
        {
            try
            {
                var json = await _cloudService.GiveUp();
                var response = JsonUtility.FromJson<GiveUpResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GiveUpResponse() { RequestSuccessful = false, Successful = false};
                return response;
            }
        }

        public static async Task<ClaimRewardsResponse> ClaimRewards()
        {
            try
            {
                var json = await _cloudService.ClaimRewards();
                var response = JsonUtility.FromJson<ClaimRewardsResponse>(json);
                response.RequestSuccessful = true;
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new ClaimRewardsResponse() { RequestSuccessful = false, Successful = false};
                return response;
            }
        }
    }

    [Serializable]
    public class RevivePlayerResponse : BaseResponse
    {
        public bool ReviveSuccessful;
    }

    [Serializable]
    public class GiveUpResponse : BaseResponse
    {
        public bool Successful;
    }

    [Serializable]
    public class ClaimRewardsResponse : BaseResponse
    {
        public bool Successful;
    }

    [Serializable]
    public class GetLevelResponse : BaseResponse
    {
        public LevelData[] Levels;
    }

    [Serializable]
    public class LevelData
    {
        public LevelSlotData[] SlotDatas;
        public int LevelType;
    }

    [Serializable]
    public class LevelSlotData
    {
        public int Type;
        public string ID;
        public int Amount;
    }
    
    [Serializable]
    public class SpinWheelResponse : BaseResponse
    {
        public int SlotIndex;
    }

}