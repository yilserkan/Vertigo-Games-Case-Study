using UnityEngine;

namespace CardGame.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/ItemData", fileName = "ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        public Sprite Sprite;
        public string Name;
        public ItemType Type;
        public ItemRarity Rarity;
        public string ID;

        public void SetRemoteConfigData(RemoteConfigItemData data)
        {
            Name = data.Name;
            Type = (ItemType)data.Type;
            Rarity = (ItemRarity)data.Rarity;
        }
    }

    public enum ItemType
    {
        Costume,
        Equipment,
        Weapon,
        Chest,
        Bomb,
        Currency
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
}



