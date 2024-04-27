using UnityEngine;

namespace CardGame.Items
{
    
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/CurrencyItemData", fileName = "CurrencyItemData", order = 0)]
    public class CurrencyItemData : ItemData
    {
        public CurrencyType CurrencyType;
    }

    public enum CurrencyType
    {
        Money,
        Gold
    }
}