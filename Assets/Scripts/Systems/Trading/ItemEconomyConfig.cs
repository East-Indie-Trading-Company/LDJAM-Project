using UnityEngine;

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
    /// 
    /// Can be created for each item within each town to define how the item's stock and price behave over time.
    /// </summary>
    [CreateAssetMenu(menuName = "Trading/Economy/Item Economy Config", fileName = "ItemEconomy_")]
    public class ItemEconomyConfig : ScriptableObject
    {
        [Header("Target Item")]
        public ItemSO item;

        [Header("Daily Stock Change (per Town)")]
        [Tooltip("Inclusive range, applied daily per town for this item.")]
        public Vector2Int dailyDeltaRange = new Vector2Int(-10, 5); // e.g., -10..+5

        [Header("Price Changes Based on Stock")]
        [Tooltip("x = normalized stock (0..1), y = price multiplier; higher stock → lower multiplier.")]
        //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AnimationCurve.Linear.html
        public AnimationCurve stockToPriceMultiplier = AnimationCurve.Linear(0, 2f, 1, 0.5f);

        [Header("Reputation Price Effect")]
        [Tooltip("x = reputation (-1..+1), y = price multiplier (e.g., high rep → cheaper).")]
        //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AnimationCurve.Linear.html
        public AnimationCurve reputationToPriceMultiplier = AnimationCurve.Linear(-1, 1.5f, 1, 0.5f);
    }
}
