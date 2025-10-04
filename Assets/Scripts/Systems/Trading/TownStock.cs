using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(menuName = "Trading/TownStock", fileName = "Town_")]
    /// <summary>
    /// Represents the stock of items available in a town.
    /// </summary>
    public class TownStock : ScriptableObject
    {
        public string townName = "Town";
        [TextArea] public string notes;

        [Serializable]
        ///<summary>
        /// Defines an item entry in the market, including stock and pricing overrides.
        /// </summary>
        public class MarketEntry
        {
            public ItemSO item;
            [Min(0)] public int stock = 0;

            [Header("Base Price (Town-Specific)")]
            [Min(0)] public int basePriceOverride = 0;     // 0 => fall back to item.baseValue
            public ItemEconomyConfig itemEconomy;          // Per-item economy settings

            public int EffectiveBasePrice =>
                basePriceOverride > 0 ? basePriceOverride : (item != null ? item.baseValue : 0);
        }

        /// <summary>
        /// The market entries for this town, defining available items and amount of items available.
        /// </summary>
        [Header("Market")]
        public List<MarketEntry> market = new();

        /// <summary>
        /// Gets the market entry for the specified item, or null if not found.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public MarketEntry GetEntry(ItemSO item) => market.Find(m => m.item == item);

        /// <summary>
        /// Adds stock of the specified item to the market. If the item does not exist in the market, it is added.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="qty"></param>
        public void AddStock(ItemSO item, int qty)
        {
            if (item == null || qty <= 0) return;
            var e = GetEntry(item);
            if (e == null)
            {
                e = new MarketEntry { item = item, stock = 0 };
                market.Add(e);
            }
            e.stock += qty;
        }

        /// <summary>
        /// Removes stock of the specified item from the market. Returns true if successful, false if not enough stock or item does not exist.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="qty"></param>
        /// <returns></returns>

        public bool RemoveStock(ItemSO item, int qty)
        {
            if (item == null || qty <= 0) return false;
            var e = GetEntry(item);
            if (e == null || e.stock < qty) return false;
            e.stock -= qty; return true;
        }
    }
}
