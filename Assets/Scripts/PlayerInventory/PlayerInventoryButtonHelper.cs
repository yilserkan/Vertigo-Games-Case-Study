using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Inventory
{
    public class PlayerInventoryButtonHelper : MonoBehaviour
    {
        [SerializeField] private Button _openInventoryButton;

        public static event Action OnOpenInventoryButtonClicked;
        
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void OnValidate()
        {
            _openInventoryButton = GetComponent<Button>();
        }

        private void HandleOnOpenInventoryButtonClicked()
        {
            OnOpenInventoryButtonClicked?.Invoke();
        }
        
        private void AddListeners()
        {
            _openInventoryButton.onClick.AddListener(HandleOnOpenInventoryButtonClicked);
        }

        private void RemoveListeners()
        {
            _openInventoryButton.onClick.RemoveListener(HandleOnOpenInventoryButtonClicked);
        }
    }
}