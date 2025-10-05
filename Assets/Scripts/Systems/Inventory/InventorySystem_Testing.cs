using UnityEngine;

namespace Trading.Tests
{
    public class InventoryTest : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InventoryManager inventory;

        [Header("Test Items")]
        [SerializeField] private ItemSO[] items;

        [ContextMenu("Run Inventory Tests")]
        public void RunTests()
        {
            if (!inventory) inventory = InventoryManager.Instance;
            if (!inventory) { Debug.LogError("InventoryManager missing!"); return; }

            Debug.Log("*** InventoryManager Test ***");

            // Start clean
            inventory.ClearAll();
            inventory.AddGold(100);
            Debug.Log($"Start Gold: {inventory.Gold}");

            foreach (var item in items)
            {
                inventory.AddItem(item, 5);
                Debug.Log($"Added 5x {item.itemName}. Qty now: {inventory.GetQuantity(item)}");
            }

            // Remove one item from first slot
            if (items.Length > 0)
            {
                var firstItem = items[0];
                inventory.RemoveItem(firstItem, 2);
                Debug.Log($"Removed 2x {firstItem.itemName}. Qty now: {inventory.GetQuantity(firstItem)}");
            }

            inventory.RemoveGold(40);
            Debug.Log($"End Gold: {inventory.Gold}");

            Debug.Log("*** End of Test ***");
        }
    }
}
