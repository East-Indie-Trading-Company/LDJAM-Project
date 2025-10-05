// Scripts/Systems/Inventory/InventoryManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trading
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        /// <summary>
        /// A stack of items, containing the item reference and the quantity owned.
        /// </summary>
        [Serializable]
        public class ItemStack
        {
            public ItemSO item;
            public int quantity;
        }

        /// <summary>
        /// The player's current gold.
        /// </summary>
        [Header("Player Wallet")]
        [SerializeField] private int gold = 0;

        /// <summary> The player's current inventory of items. </summary>
        [Header("Player Inventory")]
        [SerializeField] private List<ItemStack> items = new();

        /// <summary> The player's current gold. </summary>
        public int Gold => gold;

        /// <summary> The player's current inventory of items. </summary>
        public IReadOnlyList<ItemStack> Items => items;

        /// <summary>
        /// Ensures only one instance of the InventoryManager exists.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gold = 500; // Starting gold
        }

        /// <summary>
        /// Adds gold to the player's inventory.
        /// </summary>
        public void AddGold(int amount)
        {
            gold = Mathf.Max(0, gold + Mathf.Max(0, amount));
            GameManager.Instance?.onCurrencyChanged.Invoke(gold);
        }

        /// <summary>
        /// Attempts to remove gold from the player's inventory. Returns true if successful.
        /// </summary>
        public bool RemoveGold(int amount)
        {
            amount = Mathf.Max(0, amount);
            GameManager.Instance?.onCurrencyChanged.Invoke(gold);
            if (gold < amount) return false;
            gold -= amount; return true;
        }

        /// <summary>
        /// Gets the quantity of a specific item in the player's inventory.
        /// </summary>
        /// <param name="item">The scriptable object of the item to check</param>
        /// <returns>The quantity of the item in the inventory</returns>
        public int GetQuantity(ItemSO item)
        {
            var stack = items.Find(s => s.item == item);
            return stack == null ? 0 : Mathf.Max(0, stack.quantity);
        }

        /// <summary>
        /// Adds a specific quantity of an item to the player's inventory.
        /// </summary>
        /// <param name="item">The scriptable object of the item to add</param>
        /// <param name="qty">The quantity of the item to add</param>
        public void AddItem(ItemSO item, int qty)
        {
            if (item == null || qty <= 0) return;
            var stack = items.Find(s => s.item == item);
            if (stack == null) items.Add(new ItemStack { item = item, quantity = qty });
            else stack.quantity += qty;
        }

        /// <summary>
        /// Attempts to remove a specific quantity of an item from the player's inventory. Returns true if successful.
        /// </summary>
        /// <param name="item">The scriptable object of the item to remove</param>
        /// <param name="qty">The quantity of the item to remove</param>
        /// <returns>True if the item was successfully removed, false otherwise</returns>
        public bool RemoveItem(ItemSO item, int qty)
        {
            if (item == null || qty <= 0) return false;
            var stack = items.Find(s => s.item == item);
            if (stack == null || stack.quantity < qty) return false;
            stack.quantity -= qty;
            if (stack.quantity <= 0) items.Remove(stack);
            return true;
        }

        /// <summary>
        /// Clears all items and gold from the player's inventory. Moreso for bug handling and won't be used in normal gameplay.
        /// </summary>
        public void ClearAll()
        {
            gold = 0;
            GameManager.Instance?.onCurrencyChanged.Invoke(gold);
            items.Clear();
        }
    }
}
