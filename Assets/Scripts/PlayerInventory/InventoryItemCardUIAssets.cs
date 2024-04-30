using CardGame.Items;
using UnityEngine;

namespace CardGame.Inventory
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Inventory/ItemCardUIAssets", fileName = "InventoryItemCardUIAssets", order = 0)]
    public class InventoryItemCardUIAssets : ScriptableObject
    {
        [SerializeField] private Color[] _cardBgColor;
        [SerializeField] private Color[] _cardFrameColor;
        [SerializeField] private Color[] _cardItemNameTextColor;
        
        public bool TryGetCardBgColor(ItemRarity rarity, out Color color)
        {
            int rarityIndex = (int)rarity;
            if (_cardBgColor.Length <= rarityIndex)
            {
                color = Color.grey;
                return false;
            }
    
            color = _cardBgColor[rarityIndex];
            return true;
        }
        
        public bool TryGetCardFrameColor(ItemRarity rarity, out Color color)
        {
            int rarityIndex = (int)rarity;
            if (_cardFrameColor.Length <= rarityIndex)
            {
                color = Color.grey;
                return false;
            }
    
            color = _cardFrameColor[rarityIndex];
            return true;
        }
    
        public bool TryGetItemNameTextColor(ItemRarity rarity, out Color color)
        {
            int rarityIndex = (int)rarity;
            if (_cardItemNameTextColor.Length <= rarityIndex)
            {
                color = Color.grey;
                return false;
            }
    
            color = _cardItemNameTextColor[rarityIndex];
            return true;
        }
    }
}