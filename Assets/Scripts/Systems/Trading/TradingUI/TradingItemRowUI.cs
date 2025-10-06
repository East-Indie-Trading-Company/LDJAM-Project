using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading
{
    // Row layout:
    // [Icon] [Name]      [Town Stock • Buy] 
    //                    [Your Qty  • Sell]   [Qty][Buy][Sell]
    public class TradingItemRowUI : MonoBehaviour
    {
        [Header("UI - Visuals")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;

        [Header("UI - Info")]
        [SerializeField] private TMP_Text infoLine1;
        [SerializeField] private TMP_Text infoLine2;

        [Header("UI - Actions")]
        [SerializeField] private TMP_InputField quantityInput;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button sellButton;

        // Wiring
        private Trading.TradingUI panel;
        private TradingManager trading;
        private InventoryManager inventory;
        private TownStock town;
        private ItemSO item;

        // ---- COMPAT SHIMS ----
        // Many callers use Init/RefreshRow; others use init/Refresh. Support both.

        public void Init(Trading.TradingUI panel, TradingManager trading, InventoryManager inventory, TownStock town, ItemSO item)
            => init(panel, trading, inventory, town, item);

        public void RefreshRow() => Refresh();

        // ---- Original API (kept) ----
        public void init(Trading.TradingUI panel, TradingManager trading, InventoryManager inventory, TownStock town, ItemSO item)
        {
            this.panel     = panel;
            this.trading   = trading;
            this.inventory = inventory;
            this.town      = town;
            this.item      = item;

            if (buyButton != null)  buyButton.onClick.AddListener(HandleBuyClicked);
            if (sellButton != null) sellButton.onClick.AddListener(HandleSellClicked);

            // Static visuals
            if (nameText != null)  nameText.text = item != null ? item.itemName : "";
            if (iconImage != null) iconImage.sprite = item != null ? item.icon : null;

            refreshUI();
        }

        /// <summary>Public refresh used by panel after trades/day advance.</summary>
        public void Refresh() => refreshUI();

        private void refreshUI()
        {
            if (item == null || town == null || trading == null || inventory == null) return;

            var entry = town.GetEntry(item);
            if (entry == null) return;

            int townStock = entry.stock;
            int buyPrice  = trading.GetBuyPrice(town, item);   // player pays
            int sellPrice = trading.GetSellPrice(town, item);  // player receives
            int playerQty = inventory.GetQuantity(item);

            if (infoLine1 != null)
                infoLine1.text = $"Town Stock [{townStock}] • Buy [{buyPrice}]";

            if (infoLine2 != null)
                infoLine2.text = $"You Have [{playerQty}] • Sell [{sellPrice}]";

            if (quantityInput != null && string.IsNullOrWhiteSpace(quantityInput.text))
                quantityInput.text = "1";
        }

        private int GetTransactionQuantityOrDefault()
        {
            if (quantityInput == null) return 1;
            if (int.TryParse(quantityInput.text, out var q)) return Mathf.Max(1, q);
            return 1;
        }

        private void HandleBuyClicked()
        {
            if (trading == null || inventory == null || town == null || item == null) return;

            int q = GetTransactionQuantityOrDefault();
            if (trading.BuyFromTown(town, item, q, out var reason))
            {
                panel?.RefreshAll();
                // TODO(Dialogue): merchant bark hook
            }
            else
            {
                Debug.LogWarning($"[Trading] Buy failed: {reason}");
            }
        }

        private void HandleSellClicked()
        {
            if (trading == null || inventory == null || town == null || item == null) return;

            int q = GetTransactionQuantityOrDefault();
            if (trading.SellToTown(town, item, q, out var reason))
            {
                panel?.RefreshAll();
                // TODO(Dialogue): merchant bark hook
            }
            else
            {
                Debug.LogWarning($"[Trading] Sell failed: {reason}");
            }
        }
    }
}
