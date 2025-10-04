using UnityEngine;
using Trading;

namespace Trading
{

    /// <summary>
    /// ItemSO
    /// ↓
    /// ItemEconomyConfig
    /// ↓
    /// TownStock
    /// ↓
    /// EconomyManager.AdvanceDay
    /// ↓
    /// PriceCalculator.GetBuy/SellPrice()
    /// ↓
    /// TradingManager
    /// ↓
    /// Player Inventory
    /// Can be created for each item within each town to define how the item's stock and price behave over time.
    /// </summary>
[CreateAssetMenu(menuName = "Trading/Economy/Item Economy Config", fileName = "ItemEconomy_")]
    public class ItemEconomyConfig : ScriptableObject
    {
        [Header("Target Item")]
        public ItemSO item;


        [Header("Economy Factors")]
        [Tooltip("How valuable the item is when demand is high. Higher demand = higher purchase price.")]
        [Range(0.1f, 10f)] public float demandMultiplier = 1.0f;

        [Tooltip("How much of this item is produced daily in towns. Positive values increase stock.")]
        [Range(0f, 100f)] public float productionRate = 10f;

        [Tooltip("How much of this item is consumed daily in towns. Positive values decrease stock.")]
        [Range(0f, 100f)] public float consumptionRate = 8f;

        [Header("Price Settings")]
        [Tooltip("Base purchase price multiplier used to calculate the town's daily adjusted price.")]
        [Range(0.1f, 5f)] public float purchasePriceMultiplier = 1.0f;

        [Tooltip("Maximum and minimum stock bounds used for normalizing demand logic.")]
        public int minStock = 0;
        public int maxStock = 200;

        [Header("Base Prices (Editable by Designers)")]
        [Tooltip("Default base buy price for this item before modifiers.")]
        public int buyPrice = 1;

        [Tooltip("Default base sell price for this item before modifiers.")]
        public int sellPrice = 1;

        [TextArea] [Tooltip("Optional notes for designers or narrative writers.")]
        public string designerNotes;

        /// <summary>
        /// Called daily by the EconomyManager. Calculates the delta change in stock.
        /// </summary>
        /// <returns></returns>
        public int GetDailyStockDelta()
        {
            // Stock increases with production and decreases with consumption
            float delta = productionRate - consumptionRate;
            return Mathf.RoundToInt(delta);
        }

        /// <summary>
        /// Calculates a price multiplier based on demand and remaining stock.
        /// Lower stock = higher price (simulated demand effect).
        /// WIP - Will change as a formula is set in stone
        /// </summary>
        /// <param name="currentStock"></param>
        /// <returns></returns>
        public float GetDynamicPriceMultiplier(int currentStock)
        {
            if (maxStock <= 0) return 1f;
            float normalizedStock = Mathf.Clamp01((float)(currentStock - minStock) / (maxStock - minStock));

            // Inverse relationship: less stock → higher price
            float scarcity = 1f - normalizedStock;
            return Mathf.Max(0.1f, purchasePriceMultiplier * (1f + scarcity * demandMultiplier));
        }
    }
}
