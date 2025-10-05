using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading
{
    /// <summary>
    /// Builds and refreshes the market UI: populates item rows, updates a dynamic header with the current town,
    /// and shows a live-updating player gold label anchored at the top-right.
    /// </summary>
    public class TradingUI : MonoBehaviour
    {
        [Header("Wiring (auto if left empty)")]
        [SerializeField] private RectTransform rowsParent;
        [SerializeField] private GameObject rowPrefab;

        [Header("Header & Gold")]
        [SerializeField] private TMP_Text headerTitleText;
        [SerializeField] private TMP_Text playerGoldText;
        [SerializeField] private string headerFormat = "Market Interface";
        [SerializeField] private string goldFormat = "Gold: {0}";

        [Header("Defaults")]
        [SerializeField] private TownStock defaultTown;

        [Header("Row Config")]
        [SerializeField] private float defaultRowHeight = 80f;

        private TownStock town;
        private readonly List<TradingItemRowUI> liveRows = new();

        private TradingManager trading;
        private InventoryManager inventory;

        /// <summary>
        /// Performs best-effort auto-wiring for required UI elements and resolves managers.
        /// </summary>
        private void Awake()
        {
            TryAutoFindRowsParent();
            TryAutoFindHeaderAndGold();

            if (!trading) trading = TradingManager.Instance;
            if (!inventory) inventory = InventoryManager.Instance;
        }

        /// <summary>
        /// Ensures a town is present and triggers initial build.
        /// </summary>
        private void Start()
        {
            if (town == null) town = defaultTown;
            Rebuild();
        }

        /// <summary>
        /// Swaps the active town and rebuilds the list.
        /// </summary>
        public void SetTown(TownStock newTown)
        {
            if (newTown == town) return;
            town = newTown;
            Rebuild();
        }

        /// <summary>
        /// Redraws rows and updates header/gold labels.
        /// </summary>
        public void RefreshAll()
        {
            for (int i = 0; i < liveRows.Count; i++)
                if (liveRows[i] != null) liveRows[i].Refresh();

            UpdateHeaderUI();
            UpdateGoldUI();
        }

        /// <summary>
        /// Rebuilds the market list from the active town and updates header/gold.
        /// </summary>
        public void Rebuild()
        {
            if (trading == null) trading = TradingManager.Instance;
            if (inventory == null) inventory = InventoryManager.Instance;

            if (!rowsParent) TryAutoFindRowsParent();
            ClearChildren(rowsParent);
            liveRows.Clear();

            UpdateHeaderUI();
            UpdateGoldUI();

            if (town == null) return;
            if (town.market == null || town.market.Count == 0) return;

            foreach (var entry in town.market)
            {
                if (entry == null || entry.item == null) continue;
                if (entry.itemEconomy == null) continue;

                GameObject go;
                TradingItemRowUI row;

                if (rowPrefab)
                {
                    go = Instantiate(rowPrefab, rowsParent);
                    var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
                    if (le.preferredHeight <= 0)
                    {
                        le.preferredHeight = defaultRowHeight;
                        le.minHeight = defaultRowHeight;
                    }
                    row = go.GetComponent<TradingItemRowUI>() ?? go.AddComponent<TradingItemRowUI>();
                }
                else
                {
                    go = CreateFallbackRow(rowsParent, entry.item);
                    row = go.GetComponent<TradingItemRowUI>();
                }

                row.init(panel: this, trading: trading, inventory: inventory, town: town, item: entry.item);
                liveRows.Add(row);
            }
        }

        /// <summary>
        /// Hook for rows or external systems to indicate a completed transaction.
        /// </summary>
        public void OnTransactionCompleted()
        {
            RefreshAll();
        }

        /// <summary>
        /// Updates the header title using the current town and configured format.
        /// </summary>
        private void UpdateHeaderUI()
        {
            if (!headerTitleText) return;
            var name = town?.townName ?? "Market";
            headerTitleText.text = string.Format(headerFormat, name);
        }

        /// <summary>
        /// Updates the player gold label using the current inventory and configured format.
        /// </summary>
        private void UpdateGoldUI()
        {
            if (!playerGoldText) return;
            var gold = inventory ? inventory.Gold : 0;
            playerGoldText.text = string.Format(goldFormat, gold);
        }

        /// <summary>
        /// Attempts to find common ScrollView Content transforms and bind them as the rows parent.
        /// </summary>
        private void TryAutoFindRowsParent()
        {
            if (rowsParent) return;

            Transform content = transform.Find("MarketScrollView/Viewport/Content")
                               ?? transform.Find("RightColumn/MarketScrollView/Viewport/Content")
                               ?? transform.Find("Viewport/Content");

            if (!content)
            {
                var scroll = GetComponentInChildren<ScrollRect>(true);
                if (scroll && scroll.content) content = scroll.content.transform;
            }

            rowsParent = content ? content.GetComponent<RectTransform>() : null;
        }

        /// <summary>
        /// Binds existing header and gold text elements if present; creates a top-right gold label if none is found.
        /// </summary>
        private void TryAutoFindHeaderAndGold()
        {
            if (!headerTitleText)
            {
                var t = transform.Find("RightColumn/Header/Title") ?? transform.Find("Header/Title");
                headerTitleText = t ? t.GetComponent<TMP_Text>() : null;
            }

            if (!playerGoldText)
            {
                var t = transform.Find("RightColumn/Header/Gold") ?? transform.Find("Header/Gold");
                playerGoldText = t ? t.GetComponent<TMP_Text>() : null;
            }

            if (!playerGoldText)
            {
                var canvasRoot = GetComponentInParent<Canvas>()?.transform ?? transform;
                var go = new GameObject("GoldLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
                go.transform.SetParent(canvasRoot, false);

                var rt = go.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(1f, 1f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.pivot = new Vector2(1f, 1f);
                rt.anchoredPosition = new Vector2(-24f, -24f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 260f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40f);

                var tmp = go.GetComponent<TextMeshProUGUI>();
                tmp.enableAutoSizing = true;
                tmp.alignment = TextAlignmentOptions.Right;
                tmp.raycastTarget = false;

                playerGoldText = tmp;
            }
        }

        /// <summary>
        /// Destroys all children of a RectTransform.
        /// </summary>
        private static void ClearChildren(RectTransform rt)
        {
            if (!rt) return;
            for (int i = rt.childCount - 1; i >= 0; i--)
                Object.Destroy(rt.GetChild(i).gameObject);
        }

        /// <summary>
        /// Creates a minimal, code-built row with a TradingItemRowUI and basic visuals for fallback use.
        /// </summary>
        private GameObject CreateFallbackRow(RectTransform parent, ItemSO item)
        {
            var row = new GameObject("Row_Fallback", typeof(RectTransform));
            row.transform.SetParent(parent, false);

            var leRow = row.AddComponent<LayoutElement>();
            leRow.preferredHeight = defaultRowHeight;
            leRow.minHeight = defaultRowHeight;

            var hl = row.AddComponent<HorizontalLayoutGroup>();
            hl.spacing = 12;
            hl.childAlignment = TextAnchor.MiddleLeft;

            var iconRT = new GameObject("Icon", typeof(RectTransform)).GetComponent<RectTransform>();
            iconRT.SetParent(row.transform, false);
            iconRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48);
            iconRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 48);
            var iconImg = iconRT.gameObject.AddComponent<Image>();
            iconImg.sprite = item ? item.icon : null;
            var leIcon = iconRT.gameObject.AddComponent<LayoutElement>();
            leIcon.preferredWidth = 48;

            var nameGO = new GameObject("Name", typeof(RectTransform));
            nameGO.transform.SetParent(row.transform, false);
            var nameTMP = nameGO.AddComponent<TextMeshProUGUI>();
            nameTMP.text = item ? item.itemName : "(Item)";
            nameTMP.fontSize = 28;
            nameTMP.color = Color.black;
            var leName = nameGO.AddComponent<LayoutElement>();
            leName.flexibleWidth = 1;

            var infoGO = new GameObject("Info", typeof(RectTransform));
            infoGO.transform.SetParent(row.transform, false);
            var infoTMP = infoGO.AddComponent<TextMeshProUGUI>();
            infoTMP.text = "Sell [X] at [Price]";
            infoTMP.fontSize = 24;
            infoTMP.color = new Color(0, 0, 0, 0.85f);

            var btnGO = new GameObject("Btn_Buy", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGO.transform.SetParent(row.transform, false);
            var img = btnGO.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 0.15f);
            var labelGO = new GameObject("Label", typeof(RectTransform));
            labelGO.transform.SetParent(btnGO.transform, false);
            var label = labelGO.AddComponent<TextMeshProUGUI>();
            label.text = "Buy";
            label.fontSize = 24;
            label.color = Color.black;
            label.alignment = TextAlignmentOptions.Center;
            var rt = btnGO.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
            var leBtn = btnGO.AddComponent<LayoutElement>();
            leBtn.preferredWidth = 100;

            row.AddComponent<TradingItemRowUI>();
            return row;
        }
    }
}
