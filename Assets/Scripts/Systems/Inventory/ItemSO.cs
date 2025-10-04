using UnityEngine;

namespace Trading
{

    [CreateAssetMenu(menuName = "Trading/Items/ItemSO", fileName = "ItemSO_")]
    public class ItemSO : ScriptableObject
    {
        /// <summary>
        /// The name, description, and icon of the item. Visible features to be shown in UI.
        /// </summary>
        [Header("Display")]
        public string itemName; // Name of the item with Town
        public string displayName; // Name of the item without Town
        [TextArea] public string description;
        public Sprite icon;

        /// <summary>
        /// The base value and category of the item. These are used for trading calculations.
        /// </summary>
        [Header("Base Economy Anchor")]
        [Min(0)] public int baseValue = 1;    // Towns will override per-town

        public ItemCategory category = ItemCategory.Material;
        public string[] tags;
    }

    public enum ItemCategory { Weapon, Food, Material, Potion, Misc }
}
