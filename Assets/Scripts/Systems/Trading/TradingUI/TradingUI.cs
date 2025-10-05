using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading
{
    // Builds the "Market Interface" column.
    public class TradingUI : MonoBehaviour
    {
        [Header("Wiring (auto if left empty)")]
        [SerializeField] private RectTransform rowsParent; 	 // ScrollView/Content
        [SerializeField] private GameObject rowPrefab; 		 // prefab with TradingItemRowUI
        
        // Optional: Reference to display town name (set by builder)
        [SerializeField] private TMP_Text townNameText; 

        [Header("Defaults")]
        [SerializeField] private TownStock defaultTown;

        [Header("Row Config")]
        [Tooltip("The fixed height for each row. Used if the rowPrefab does not have a LayoutElement with a preferredHeight.")]
        [SerializeField] private float defaultRowHeight = 80f;

        // Runtime
        private TownStock town;
        private readonly List<TradingItemRowUI> liveRows = new();

        // Managers (resolved at runtime)
        private TradingManager trading;
        private InventoryManager inventory;

        // -------------------------------

        private void Awake()
        {
            // Robust auto-find for rowsParent
            TryAutoFindRowsParent();

            // Auto-resolve managers if they aren't manually set in the inspector
            if (!trading)   trading   = TradingManager.Instance;
            if (!inventory) inventory = InventoryManager.Instance;
        }

        private void OnEnable()
        {
            if (town == null) town = defaultTown;
        }


        private void Start()
        {
            // Ensure at least one rebuild on scene start
            if (town == null) town = defaultTown;
            Rebuild();
        }

        /// <summary>Swap the UI to a new town (player traveled).</summary>
        public void SetTown(TownStock newTown)
        {
            if (newTown == town) return;
            town = newTown;
            Rebuild();
        }

        /// <summary>External systems can ask the list to redraw after changes (buy/sell/day-advance).</summary>
        public void RefreshAll()
        {
            for (int i = 0; i < liveRows.Count; i++)
                if (liveRows[i] != null) liveRows[i].Refresh();
        }

        // -------------------------------

        public void Rebuild()
        {
            // Update the town name display
            if (townNameText != null)
            {
                townNameText.text = town != null ? town.townName.ToUpper() : "MARKET CLOSED";
            }
            
            // Re-resolve managers in case they were created after Awake
            if (trading == null) 	trading 	= TradingManager.Instance;
            if (inventory == null) 	inventory 	= InventoryManager.Instance;

            // Guards + visible hints (don’t fail silently)
            if (!rowsParent)
            {
                TryAutoFindRowsParent();
            }

            ClearChildren(rowsParent);

            if (!town)
            {
                return;
            }
            if (town.market == null || town.market.Count == 0)
            {
                Debug.LogWarning("[TradingUI] This town has no market entries.");
                return;
            }

            int made = 0, skippedNoItem = 0, skippedNoConfig = 0;

            liveRows.Clear();

            foreach (var entry in town.market)
            {
                if (entry == null || entry.item == null) { skippedNoItem++; continue; }
                if (entry.itemEconomy == null) 			{ skippedNoConfig++; continue; }

                GameObject go;
                TradingItemRowUI row;

                if (rowPrefab)
                {
                    go = Instantiate(rowPrefab, rowsParent);
                    
                    // --- FIX: Ensure the row has a LayoutElement with a non-zero preferredHeight ---
                    // Get the existing LayoutElement or add a new one.
                    var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();

                    // IMPORTANT: If preferredHeight is 0 or less, set it to the defaultRowHeight (80f).
                    // This is necessary because the parent VLG requires this value to size the row.
                    if (le.preferredHeight <= 0)
                    {
                        le.preferredHeight = defaultRowHeight; 
                        // Set minHeight as well for maximum Layout Group compatibility.
                        le.minHeight = defaultRowHeight;
                        Debug.LogWarning($"[TradingUI] Force-set LayoutElement preferredHeight to {defaultRowHeight} for {entry.item.itemName}. Check your row prefab!");
                    }
                    // --- END FIX ---
                    
                    row = go.GetComponent<TradingItemRowUI>();
                    if (!row) row = go.AddComponent<TradingItemRowUI>(); // safety
                }
                else
                {
                    // Fallback path already handles LayoutElement creation with height
                    go 	= CreateFallbackRow(rowsParent, entry.item);
                    row = go.GetComponent<TradingItemRowUI>();
                }

                // Your row exposes init() and Refresh(); builders sometimes call Init/RefreshRow.
                // We'll call the canonical init() here—row has shims if needed.
                row.init(panel: this, trading: trading, inventory: inventory, town: town, item: entry.item);
                liveRows.Add(row);
                made++;
            }

            if (made == 0)
            {
                // Nothing got built—explain why
                string why = $"No rows built.\n" +
                             (skippedNoItem 	> 0 ? $"- {skippedNoItem} entries missing ItemSO\n" : "") +
                             (skippedNoConfig 	> 0 ? $"- {skippedNoConfig} entries missing ItemEconomyConfig\n" : "") +
                             "Check the TownStock.asset → market list.";
            }
        }
        
        // --- Transaction Callbacks (Public for buttons to bind to) ---

        public void OnTransactionCompleted()
        {
            // Called after BuyFromTown or SellToTown completes successfully
            RefreshAll();
        }

        // ------------------------------- helpers

        private void TryAutoFindRowsParent()
        {
            if (rowsParent) return;

            // 1) Common local paths
            Transform content = transform.Find("MarketScrollView/Viewport/Content")
                               ?? transform.Find("RightColumn/MarketScrollView/Viewport/Content")
                               ?? transform.Find("Viewport/Content");

            // 2) Fallback: scan children for a ScrollRect & use its Content
            if (!content)
            {
                var scroll = GetComponentInChildren<ScrollRect>(true);
                if (scroll && scroll.content) content = scroll.content.transform;
            }

            rowsParent = content ? content.GetComponent<RectTransform>() : null;
        }

        private static void ClearChildren(RectTransform rt)
        {
            if (!rt) return;
            for (int i = rt.childCount - 1; i >= 0; i--)
                Object.Destroy(rt.GetChild(i).gameObject);
        }

        private GameObject CreateFallbackRow(RectTransform parent, ItemSO item)
        {
            var row = new GameObject("Row_Fallback", typeof(RectTransform));
            row.transform.SetParent(parent, false);

            var leRow = row.AddComponent<LayoutElement>();
            leRow.preferredHeight = defaultRowHeight; // Set height for the fallback row
            leRow.minHeight = defaultRowHeight; // Also set minHeight for robustness

            var hl = row.AddComponent<HorizontalLayoutGroup>();
            hl.spacing = 12; hl.childAlignment = TextAnchor.MiddleLeft;

            // Icon
            var iconRT = new GameObject("Icon", typeof(RectTransform)).GetComponent<RectTransform>();
            iconRT.SetParent(row.transform, false);
            iconRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48);
            iconRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 	48);
            var iconImg = iconRT.gameObject.AddComponent<Image>();
            iconImg.sprite = item ? item.icon : null;
            var leIcon = iconRT.gameObject.AddComponent<LayoutElement>();
            leIcon.preferredWidth = 48; // Ensure icon takes its space

            // Name
            var nameGO = new GameObject("Name", typeof(RectTransform));
            nameGO.transform.SetParent(row.transform, false);
            var nameTMP = nameGO.AddComponent<TextMeshProUGUI>();
            nameTMP.text = item ? item.itemName : "(Item)";
            nameTMP.fontSize = 28;
            nameTMP.color = Color.white;
            var leName = nameGO.AddComponent<LayoutElement>();
            leName.flexibleWidth = 1; // Allow name to take remaining space

            // Info (single label; row supports single or split)
            var infoGO = new GameObject("Info", typeof(RectTransform));
            infoGO.transform.SetParent(row.transform, false);
            var infoTMP = infoGO.AddComponent<TextMeshProUGUI>();
            infoTMP.text = "Sell [X] at [Price]";
            infoTMP.fontSize = 24;
            infoTMP.color = new Color(1,1,1,0.85f);

            // Buy button
            var btnGO = new GameObject("Btn_Buy", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGO.transform.SetParent(row.transform, false);
            var img = btnGO.GetComponent<Image>(); img.color = new Color(1,1,1,0.15f);
            var labelGO = new GameObject("Label", typeof(RectTransform));
            labelGO.transform.SetParent(btnGO.transform, false);
            var label = labelGO.AddComponent<TextMeshProUGUI>();
            label.text = "Buy";
            label.fontSize = 24;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;
            var rt = btnGO.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 	40);
            var leBtn = btnGO.AddComponent<LayoutElement>();
            leBtn.preferredWidth = 100;

            // Attach TradingItemRowUI for logic wiring
            row.AddComponent<TradingItemRowUI>();
            return row;
        }
    }
}
