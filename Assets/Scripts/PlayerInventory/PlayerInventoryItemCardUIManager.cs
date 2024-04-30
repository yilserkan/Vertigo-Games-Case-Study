using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Inventory
{
    public class PlayerInventoryItemCardUIManager : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _itemFrameImage;
        [SerializeField] private Image _itemBgImage;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private InventoryItemCardUIAssets _cardUIAssets;
    
        public void InitializeItem(ItemData playerInventoryData, float amount)
        {
            _itemImage.sprite = playerInventoryData.Sprite;
            _itemName.text = $"{playerInventoryData.Name}";
            _itemAmount.text = $"{amount}";

            var rarity = playerInventoryData.Rarity;
            if (_cardUIAssets.TryGetCardBgColor(rarity, out var bgColor))
                _itemBgImage.color = bgColor;
            if (_cardUIAssets.TryGetCardFrameColor(rarity, out var frameColor))
                _itemFrameImage.color = frameColor;
            if (_cardUIAssets.TryGetItemNameTextColor(rarity, out var textColor))
                _itemName.color = textColor;
        }
    }
}