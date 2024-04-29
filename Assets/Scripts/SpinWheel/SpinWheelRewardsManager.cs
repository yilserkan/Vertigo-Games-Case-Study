using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.CloudServices;
using CardGame.CloudServices.InventoryService;
using CardGame.Inventory;
using CardGame.Items;
using CardGame.ServiceManagement;
using UnityEngine;

namespace CardGame.SpinWheel
{
    public class SpinWheelRewardsManager : MonoBehaviour
    {
        [SerializeField] private SpinWheelRewardItem _rewardItemPrefab;
        [SerializeField] private Transform _rewardItemParent;
        [SerializeField] private RectTransform _verticalLayoutRect;
        
        private ItemContainers _itemContainer;
        private SpinWheelParticleManager _particleManager;
        private Dictionary<string, SpinWheelRewardItem> _wheelRewards;

        public static event Action OnRewardParticlesCompleted; 
        
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
            ServiceLocator.LazyGlobal.Get(out _itemContainer);
            ServiceLocator.For(this)?.Get(out _particleManager);
            _wheelRewards = new Dictionary<string, SpinWheelRewardItem>();
        }

        private void AddReward(LevelSlotData data)
        {
            if (!_itemContainer.TryGetItemData((ItemType)data.Type, data.ID, out var itemData)) { return; }
            
            if (_wheelRewards.ContainsKey(data.ID))
            {
                SpawnParticles(data, itemData);
            }
            else
            {
                CreateNewReward(data, itemData);
            }
        }
   
        private void CreateNewReward(LevelSlotData data, ItemData itemData)
        {
            var instantiated = Instantiate(_rewardItemPrefab, _rewardItemParent);
            instantiated.SetReward(itemData.Sprite);
            _wheelRewards.Add(data.ID, instantiated);
            SpawnParticles(data, itemData);
        }
        
        private void UpdateReward(LevelSlotData data)
        {
            _wheelRewards[data.ID].UpdateAmount(data.Amount);
            PlayerInventory.AddToWheelRewards(new PlayerInventoryData(){ID = data.ID, Amount = data.Amount, Type = data.Type});
        }

        private void SpawnParticles(LevelSlotData data, ItemData itemData)
        {
            var targetRewardRect = _wheelRewards[data.ID].GetImageRect();
            _particleManager.SpawnParticles(targetRewardRect , itemData.Sprite,() => HandleOnRewardParticlesCompleted(data));
        }

        private void HandleOnRewardParticlesCompleted(LevelSlotData data)
        {
            UpdateReward(data);
            OnRewardParticlesCompleted?.Invoke();
        }

        private void ClearRewards()
        {
            foreach (var reward in _wheelRewards)
            {
                Destroy(reward.Value.gameObject);
            }
            _wheelRewards.Clear();
        }
        
        private void AddListeners()
        {
            LevelManager.OnStartGame += ClearRewards;
            LevelManager.OnRewardClaimed += AddReward;
        }

        private void RemoveListeners()
        {
            LevelManager.OnStartGame -= ClearRewards;
            LevelManager.OnRewardClaimed -= AddReward;
        }
    }
}