using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.SpinWheel
{
    public static class SpinWheelCloudRequests
    {
        private static ISpinWheelCloudService _cloudService = new MockSpinWheelCloudService();
        
        public static async Task<string> GetLevelData()
        {
            return await _cloudService.GetLevelData();
        }

        public static async Task<string> SpinWheel()
        {
            return await _cloudService.SpinWheel();
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
    }

    public interface ISpinWheelCloudService
    {
        public Task<string> GetLevelData();
        public Task<string> SpinWheel();
    }

    [Serializable]
    public class SpinWheelResponse
    {
        public int SlotIndex;
    }
}