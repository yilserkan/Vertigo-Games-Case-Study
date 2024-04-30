using System.Collections.Generic;
using System.Linq;
using CardGame.Extensions;
using CardGame.Items;
using CardGame.ServiceManagement;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Inventory
{
    public class PlayerInventoryUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryParent;
        [SerializeField] private PlayerInventoryItemCardUIManager _cardPrefab;
        [SerializeField] private Transform _cardParent;
        [SerializeField] private Button _closeButton;

        private ItemContainers _itemContainers;
        private List<PlayerInventoryItemCardUIManager> _cardUIManagers = new();
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
            ServiceLocator.Global.OrNull()?.Get(out _itemContainers);
        }
        
        private void OpenInventory()
        {
            SetupInventory();
            EnableInventoryPanel(true);
        }

        private void CloseInventory()
        {
            EnableInventoryPanel(false);
        }
        
        private void SetupInventory()
        {
            var inventoryItems = PlayerInventory.InventoryItemDatas;
            List<ItemData> _itemDatas = new List<ItemData>();
            foreach (var inventoryItem in inventoryItems)
            {
                if (_itemContainers.TryGetItemData((ItemType)inventoryItem.Value.Type, inventoryItem.Value.ID, out var data))
                {
                    _itemDatas.Add(data);
                }
            }

            var orderedItems = _itemDatas.OrderBy(item => item.Rarity).ToArray();

            var currentAmount = _cardUIManagers.Count;
            for (int i = 0; i < orderedItems.Length; i++)
            {
                var amount = inventoryItems[orderedItems[i].ID].Amount;
                if (i < currentAmount)
                {
                    _cardUIManagers[i].InitializeItem(orderedItems[i], amount);
                }
                else
                {
                    CreateNewCard(orderedItems[i], amount); 
                }
            }
        }

        private void CreateNewCard(ItemData data, float amount)
        {
            var instantiated = Instantiate(_cardPrefab, _cardParent);
            _cardUIManagers.Add(instantiated);
            instantiated.InitializeItem(data, amount);
        }
        
        private void EnableInventoryPanel(bool isEnabled)
        {
            _inventoryParent.SetActive(isEnabled);
        }
        
        private void AddListeners()
        {
            _closeButton.onClick.AddListener(CloseInventory);
            PlayerInventoryButtonHelper.OnOpenInventoryButtonClicked += OpenInventory;
        }

        private void RemoveListeners()
        {
            _closeButton.onClick.RemoveListener(CloseInventory);
            PlayerInventoryButtonHelper.OnOpenInventoryButtonClicked -= OpenInventory;
        }
    }
}