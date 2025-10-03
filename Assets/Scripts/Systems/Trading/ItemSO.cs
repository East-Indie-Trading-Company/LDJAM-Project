using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(menuName = "Items/ItemSO", fileName = "ItemSO_")]
    public class ItemSO : ScriptableObject
    {
        /// <summary>
        /// The immediate physical properties of the item.
        /// </summary>
        [Header("Display")]
        public string itemName;
        [TextArea] public string description;
        public Sprite icon;

        /// <summary>
        /// The trading properties of the item.
        /// </summary>
        [Header("Economy")]
        [Min(0)] public int baseValue = 1;
        public ItemCategory category = ItemCategory.Material;
        public string[] tags;
    }

    public enum ItemCategory { Weapon, Food, Material, Potion, Misc }
}