using System;
using CardGame.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.SpinWheel
{
    public class SpinWheelSlotManager : MonoBehaviour
    {
        [SerializeField] private Image _slotBgImage;
        [SerializeField] private Image _slotIcon;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Color[] _bgColors;
        [SerializeField] private Color _bombColor;
        
        public void InitializeSlot(ItemData data, int amount)
        {
            _slotIcon.sprite = data.Sprite;
            _amountText.text = amount.ToString();
            _slotBgImage.color = GetBgColor(data.Rarity, data.Type);
        }

        private Color GetBgColor(ItemRarity rarity, ItemType itemType)
        {
            if (itemType == ItemType.Bomb)
            {
                return _bombColor;
            }
            
            var rarityIndex = (int)rarity;
            if (_bgColors.Length > rarityIndex)
            {
                return _bgColors[rarityIndex];
            }
            
            return Color.white;
        }
    }
}
