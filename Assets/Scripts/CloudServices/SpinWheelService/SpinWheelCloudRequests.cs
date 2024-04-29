using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace CardGame.CloudServices
{
    public static class SpinWheelCloudRequests
    {
        // private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        private static ISpinWheelCloudService _cloudService = new UGSSpinWheelCloudService();
        
        public static async Task<GetLevelResponse> GetLevelData()
        {
            var json = await _cloudService.GetLevelData();
            return JsonUtility.FromJson<GetLevelResponse>(json);
        }

        public static async Task<SpinWheelResponse> SpinWheel()
        {
            var json = await _cloudService.SpinWheel();
            return JsonUtility.FromJson<SpinWheelResponse>(json);
        }

        public static async Task<RevivePlayerResponse> Revive()
        {
            var json = await _cloudService.Revive();
            return JsonUtility.FromJson<RevivePlayerResponse>(json);
        }

        public static async Task<GiveUpResponse> GiveUp()
        {
            var json = await _cloudService.GiveUp();
            return JsonUtility.FromJson<GiveUpResponse>(json);
        }

        public static async Task<ClaimRewardsResponse> ClaimRewards()
        {
            var json = await _cloudService.ClaimRewards();
            return JsonUtility.FromJson<ClaimRewardsResponse>(json);
        }
    }

    [Serializable]
    public class RevivePlayerResponse
    {
        public bool ReviveSuccessful;
    }

    [Serializable]
    public class GiveUpResponse
    {
        public bool Successful;
    }

    [Serializable]
    public class ClaimRewardsResponse
    {
        public bool Successful;
    }

    [Serializable]
    public class GetLevelResponse
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
    public class SpinWheelResponse
    {
        public int SlotIndex;
    }

}