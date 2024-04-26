using System;
using CardGame.Items;
using UnityEngine;

namespace CardGame.SpinWheel
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelManager _spinWheelManager;
        private GetLevelResponse _levelData;
        private int _currentStage;
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Start()
        {
            StartGame();
        }

        public async void StartGame()
        {
            ResetLevel();
            _levelData = await SpinWheelCloudRequests.GetLevelData();
            _spinWheelManager.InitializeLevel(_levelData);
            _spinWheelManager.ShowStage(_currentStage);
        }

        private void ResetLevel()
        {
            _currentStage = 0;
        }
        
        private void HandleOnSpinWheelAnimCompleted(int slotIndex)
        {
            if (HasPlayerLost(slotIndex))
            {
                Debug.Log("Player Has Lost");
                ResetAllEarnings();
                return;
            }
            // TODO : 
            // Show UI Animation
            
            ShowNextStage();
        }

        private void ShowNextStage()
        {
            _currentStage++;
            _spinWheelManager.ShowStage(_currentStage);
        }
        
        private void ResetAllEarnings()
        {
            
        }

        private bool HasPlayerLost(int slotIndex)
        {
            var slotData = _levelData.LevelData[_currentStage].SlotDatas[slotIndex];
            Debug.Log("Item ID : " + slotData.ID);
            return (ItemType)slotData.Type == ItemType.Bomb;
        }

        private void AddListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted += HandleOnSpinWheelAnimCompleted;
        }

        private void RemoveListeners()
        {
            SpinWheelManager.OnSpinAnimationCompleted -= HandleOnSpinWheelAnimCompleted;
        }
    }
}