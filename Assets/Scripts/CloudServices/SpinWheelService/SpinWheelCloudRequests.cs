using System;
using System.Threading.Tasks;
using CardGame.Extensions;
using CardGame.SceneManagement;
using CardGame.ServiceManagement;
using UnityEngine;


namespace CardGame.CloudServices
{
    public static class SpinWheelCloudRequests
    {
        // private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        private static ISpinWheelCloudService _cloudService = new UGSSpinWheelCloudService();
        private static RequestLoadingHelper _loadingHelper;
        
        public static async Task<GetLevelResponse> GetLevelData()
        {
            SetLoadingHelper();
            try
            {   
                EnableLoadingPanel(true);
                var json = await _cloudService.GetLevelData();
                var response = JsonUtility.FromJson<GetLevelResponse>(json);
                response.RequestSuccessful = true;
                EnableLoadingPanel(false);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GetLevelResponse() { RequestSuccessful = false, Levels = Array.Empty<LevelData>()};
                EnableLoadingPanel(false);
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
                EnableLoadingPanel(true);
                var json = await _cloudService.Revive();
                var response = JsonUtility.FromJson<RevivePlayerResponse>(json);
                response.RequestSuccessful = true;
                EnableLoadingPanel(false);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new RevivePlayerResponse() { RequestSuccessful = false, ReviveSuccessful = false};
                EnableLoadingPanel(false);
                return response;
            }
        }

        public static async Task<GiveUpResponse> GiveUp()
        {
            try
            {
                EnableLoadingPanel(true);
                var json = await _cloudService.GiveUp();
                var response = JsonUtility.FromJson<GiveUpResponse>(json);
                response.RequestSuccessful = true;
                EnableLoadingPanel(false);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new GiveUpResponse() { RequestSuccessful = false, Successful = false};
                EnableLoadingPanel(false);
                return response;
            }
        }

        public static async Task<ClaimRewardsResponse> ClaimRewards()
        {
            try
            {
                EnableLoadingPanel(true);
                var json = await _cloudService.ClaimRewards();
                var response = JsonUtility.FromJson<ClaimRewardsResponse>(json);
                response.RequestSuccessful = true;
                EnableLoadingPanel(false);
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError("Request Error : " + e.Message);
                var response = new ClaimRewardsResponse() { RequestSuccessful = false, Successful = false};
                EnableLoadingPanel(false);
                return response;
            }
        }

        private static void EnableLoadingPanel(bool isEnabled)
        {
            _loadingHelper.OrNull()?.EnableRequestLoadingPanel(isEnabled);
        }

        private static void SetLoadingHelper()
        {
            if (_loadingHelper == null)
            {
                ServiceLocator.Global.OrNull()?.Get(out _loadingHelper);
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