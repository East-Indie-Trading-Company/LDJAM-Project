using UnityEngine;

namespace Trading
{
    public class TradingManager : MonoBehaviour
    {
        public static TradingManager Instance { get; private set; }

        [Header("Refs")]
        [SerializeField] private InventoryManager inventory;
        [SerializeField] private EconomyManager economy;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!inventory) inventory = InventoryManager.Instance;
            if (!economy)   economy   = EconomyManager.Instance;
        }

        public int GetBuyPrice(TownStock town, ItemSO item)
        {
            var e = town?.GetEntry(item);
            if (e == null) return 0;
            return PriceCalculator.GetBuyPriceForPlayer(e, economy?.InflationIndex ?? 1f);
        }

        public int GetSellPrice(TownStock town, ItemSO item)
        {
            var e = town?.GetEntry(item);
            if (e == null) return 0;
            return PriceCalculator.GetSellPriceToPlayer(e, economy?.InflationIndex ?? 1f);
        }

        public bool BuyFromTown(TownStock town, ItemSO item, int quantity, out string reason)
        {
            reason = "";
            if (!ValidateArgs(town, item, quantity, ref reason)) return false;

            var entry = town.GetEntry(item);
            if (entry.stock < quantity) { reason = "Town stock too low."; return false; }

            int unit = GetBuyPrice(town, item);
            int cost = unit * quantity;
            if (!inventory.RemoveGold(cost)) { reason = "Not enough gold."; return false; }

            town.RemoveStock(item, quantity);
            inventory.AddItem(item, quantity);
            return true;
        }

        /// <summary>
        /// Sells a specific quantity of an item to the town at the current sell price.
        /// </summary>
        /// <param name="town">The town to sell to</param>
        public bool SellToTown(TownStock town, ItemSO item, int quantity, out string reason)
        {
            reason = "";
            if (!ValidateArgs(town, item, quantity, ref reason)) return false;

            if (inventory.GetQuantity(item) < quantity) { reason = "Not enough items."; return false; }

            int unit = GetSellPrice(town, item);
            int revenue = unit * quantity;

            if (!inventory.RemoveItem(item, quantity)) { reason = "Inventory change failed."; return false; }
            town.AddStock(item, quantity);
            inventory.AddGold(revenue);
            return true;
        }

        private bool ValidateArgs(TownStock town, ItemSO item, int quantity, ref string reason)
        {
            if (town == null) { reason = "No town selected."; return false; }
            if (item == null) { reason = "No item selected."; return false; }
            if (quantity <= 0) { reason = "Quantity must be > 0."; return false; }
            var entry = town.GetEntry(item);
            if (entry == null) { reason = "Town does not trade this item."; return false; }
            return true;
        }
    }
}
