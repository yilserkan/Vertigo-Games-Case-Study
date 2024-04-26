using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public static class SpinWheelCloudRequests
    {
        private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        
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

        public static async Task GiveUp()
        {
            await _cloudService.GiveUp();
        }

        public static async Task ClaimRewards()
        {
            await _cloudService.ClaimRewards();
        }
    }

    public class MockSpinWheelCloudService : ISpinWheelCloudService
    {
        private static MockLevelCreator _levelCreator = new();
        
        public Task<string> GetLevelData()
        {
            return Task.FromResult(JsonUtility.ToJson(_levelCreator.GetMockLevelData()));
        }

        public Task<string> SpinWheel()
        {
            var randIndex = Random.Range(0, 8);
            var response = new SpinWheelResponse(){SlotIndex = randIndex};
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> Revive()
        {
            //TODO:
            // Set player state from death to alive   
            // Decrease Player Money
            var response = new RevivePlayerResponse() { ReviveSuccessfull = true };
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task GiveUp()
        {
            //TODO:
            // Reset all cached player earnings
            
            return Task.CompletedTask;
        }

        public Task ClaimRewards()
        {
            // TODO
            // Give rewards to player

            return Task.CompletedTask;
        }
    }

    public interface ISpinWheelCloudService
    {
        public Task<string> GetLevelData();
        public Task<string> SpinWheel();

        public Task<string> Revive();
        public Task GiveUp();
        public Task ClaimRewards();
    }

    [Serializable]
    public class RevivePlayerResponse
    {
        public bool ReviveSuccessfull;
    }
    
    [Serializable]
    public class GetLevelResponse
    {
        public LevelData[] LevelData;
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