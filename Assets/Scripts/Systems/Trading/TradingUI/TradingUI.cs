using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading
{
    /// <summary>
    /// Market UI builder for a specific town. Populates rows, updates a header with the town name,
    /// and shows a live player gold label. Town is set via SetTown(...) or defaultTown; never inferred.
    /// </summary>
    public class TradingUI : MonoBehaviour
    {
        [Header("Wiring (auto if left empty)")]
        [SerializeField] private RectTransform rowsParent;
        [SerializeField] private GameObject rowPrefab;

        [Header("Header & Gold")]
        [SerializeField] private TMP_Text headerTitleText;
        [SerializeField] private TMP_Text playerGoldText;
        [SerializeField] private string headerFormat = "{0} Market";
        [SerializeField] private string goldFormat   = "Gold: {0}";

        [Header("Defaults")]
        [SerializeField] private TownStock defaultTown;

        [Header("Row Config")]
        [SerializeField] private float defaultRowHeight = 80f;

        [Header("Diagnostics")]
        [SerializeField] private bool logMissingRefs = true;

        private TownStock town;
        private readonly List<TradingItemRowUI> liveRows = new();

        private TradingManager trading;
        private InventoryManager inventory;

        /// <summary>
        /// Resolves managers and UI references without choosing a town.
        /// </summary>
        private void Awake()
        {
            if (!trading)   trading   = TradingManager.Instance;
            if (!inventory) inventory = InventoryManager.Instance;

            TryAutoFindRowsParent();
            TryAutoFindHeaderAndGold();
        }

        /// <summary>
        /// Binds default town if none has been provided and builds UI.
        /// </summary>
        private void Start()
        {
            if (town == null && defaultTown != null)
            {
                town = defaultTown;
                Debug.Log($"[TradingUI] Town set from defaultTown: {town.townName}");
            }
            else if (town == null && logMissingRefs)
            {
                Debug.LogWarning("[TradingUI] No town provided. Assign defaultTown or call SetTown(...).");
            }

            Rebuild();
        }

        /// <summary>
        /// Exposes the current town.
        /// </summary>
        public TownStock CurrentTown => town;

        /// <summary>
        /// Sets the active town explicitly and rebuilds if changed.
        /// </summary>
        public void SetTown(TownStock newTown)
        {
            if (newTown == null)
            {
                if (logMissingRefs) Debug.LogWarning("[TradingUI] SetTown(null) ignored.");
                return;
            }
            if (newTown == town) return;

            town = newTown;
            Debug.Log($"[TradingUI] Town set via SetTown: {town.townName}");
            Rebuild();
        }

        /// <summary>
        /// Redraws all rows and updates header/gold.
        /// </summary>
        public void RefreshAll()
        {
            for (int i = 0; i < liveRows.Count; i++)
                if (liveRows[i] != null) liveRows[i].Refresh();

            UpdateHeaderUI();
            UpdateGoldUI();
        }

        /// <summary>
        /// Clears and rebuilds rows for the current town and updates header/gold.
        /// </summary>
        public void Rebuild()
        {
            if (trading == null)   trading   = TradingManager.Instance;
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
                    if (le.preferredHeight <= 0f)
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
        /// Transaction callback that re-syncs UI elements.
        /// </summary>
        public void OnTransactionCompleted()
        {
            RefreshAll();
        }

        /// <summary>
        /// Updates the header label based on the current town.
        /// </summary>
        private void UpdateHeaderUI()
        {
            if (!headerTitleText)
            {
                if (logMissingRefs) Debug.LogWarning("[TradingUI] Header text reference is missing.");
                return;
            }
            var name = town ? town.townName : "Market";
            headerTitleText.text = string.Format(headerFormat, name);
        }

        /// <summary>
        /// Updates the gold label from InventoryManager.
        /// </summary>
        private void UpdateGoldUI()
        {
            if (!playerGoldText)
            {
                if (logMissingRefs) Debug.LogWarning("[TradingUI] Gold text reference is missing.");
                return;
            }
            var gold = inventory ? inventory.Gold : 0;
            playerGoldText.text = string.Format(goldFormat, gold);
        }

        /// <summary>
        /// Finds a ScrollRect content for rows if not assigned.
        /// </summary>
        private void TryAutoFindRowsParent()
        {
            if (rowsParent) return;

            Transform content = transform.Find("RightColumn/MarketScrollView/Viewport/Content")
                               ?? transform.Find("LeftColumn/MarketScrollView/Viewport/Content")
                               ?? transform.Find("MarketScrollView/Viewport/Content")
                               ?? transform.Find("Viewport/Content");

            if (!content)
            {
                var scroll = GetComponentInChildren<ScrollRect>(true);
                if (scroll && scroll.content) content = scroll.content.transform;
            }

            rowsParent = content ? content.GetComponent<RectTransform>() : null;

            if (!rowsParent && logMissingRefs)
                Debug.LogWarning("[TradingUI] Could not locate rows parent. Assign it in the inspector.");
        }

        /// <summary>
        /// Binds header and gold labels; searches LeftColumn/Header/Title first, then other common paths.
        /// Creates a top-right gold label if none is found.
        /// </summary>
        private void TryAutoFindHeaderAndGold()
        {
            if (!headerTitleText)
            {
                var t =
                    transform.Find("LeftColumn/Header/Title") ??
                    transform.Find("RightColumn/Header/Title") ??
                    transform.Find("Header/Title");
                headerTitleText = t ? t.GetComponent<TMP_Text>() : null;
            }

            if (!playerGoldText)
            {
                var t =
                    transform.Find("RightColumn/Header/Gold") ??
                    transform.Find("LeftColumn/Header/Gold")  ??
                    transform.Find("Header/Gold");
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
                rt.pivot     = new Vector2(1f, 1f);
                rt.anchoredPosition = new Vector2(-24f, -24f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 260f);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   40f);

                var tmp = go.GetComponent<TextMeshProUGUI>();
                tmp.enableAutoSizing = true;
                tmp.alignment = TextAlignmentOptions.Right;
                tmp.raycastTarget = false;
                tmp.color = Color.black;

                playerGoldText = tmp;
                if (logMissingRefs) Debug.Log("[TradingUI] Created top-right Gold label.");
            }

            if (!headerTitleText && logMissingRefs)
                Debug.LogWarning("[TradingUI] Could not locate header title text. Drag it into the inspector.");
        }

        /// <summary>
        /// Destroys all children under a RectTransform.
        /// </summary>
        private static void ClearChildren(RectTransform rt)
        {
            if (!rt) return;
            for (int i = rt.childCount - 1; i >= 0; i--)
                Object.Destroy(rt.GetChild(i).gameObject);
        }

        /// <summary>
        /// Creates a minimal fallback row with TradingItemRowUI.
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
            iconRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   48);
            var iconImg = iconRT.gameObject.AddComponent<Image>();
            iconImg.sprite = item ? item.icon : null;
            var leIcon = iconRT.gameObject.AddComponent<LayoutElement>();
            leIcon.preferredWidth = 48;

            var nameGO = new GameObject("Name", typeof(RectTransform));
            nameGO.transform.SetParent(row.transform, false);
            var nameTMP = nameGO.AddComponent<TextMeshProUGUI>();
            nameTMP.text = item ? item.itemName : "(Item)";
            nameTMP.fontSize = 28;
            nameTMP.color = Color.white;
            var leName = nameGO.AddComponent<LayoutElement>();
            leName.flexibleWidth = 1;

            var infoGO = new GameObject("Info", typeof(RectTransform));
            infoGO.transform.SetParent(row.transform, false);
            var infoTMP = infoGO.AddComponent<TextMeshProUGUI>();
            infoTMP.text = "Sell [X] at [Price]";
            infoTMP.fontSize = 24;
            infoTMP.color = new Color(1, 1, 1, 0.85f);

            var btnGO = new GameObject("Btn_Buy", typeof(RectTransform), typeof(Image), typeof(Button));
            btnGO.transform.SetParent(row.transform, false);
            var img = btnGO.GetComponent<Image>();
            img.color = new Color(1, 1, 1, 0.15f);
            var labelGO = new GameObject("Label", typeof(RectTransform));
            labelGO.transform.SetParent(btnGO.transform, false);
            var label = labelGO.AddComponent<TextMeshProUGUI>();
            label.text = "Buy";
            label.fontSize = 24;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;
            var rt = btnGO.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   40);
            var leBtn = btnGO.AddComponent<LayoutElement>();
            leBtn.preferredWidth = 100;

            row.AddComponent<TradingItemRowUI>();
            return row;
        }
    }
}
