using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trading
{
    /// <summary>
    /// Market UI for a specific town. Call SetTown(...) to trigger a full rebuild.
    /// </summary>
    public class TradingUI : MonoBehaviour
    {
        [Header("Wiring (auto if left empty)")]
        [SerializeField] private RectTransform rowsParent;
        [SerializeField] private GameObject rowPrefab;

        [Header("Header (optional)")]
        [SerializeField] private TMP_Text headerTitleText;
        [SerializeField] private string headerFormat = "{0} Market";

        [Header("Row Sizing")]
        [SerializeField] private float defaultRowHeight = 80f;

        [Header("Diagnostics")]
        [SerializeField] private bool logMissingRefs = true;

        private TownStock town;
        private readonly List<TradingItemRowUI> liveRows = new();

        [SerializeField] private TradingManager trading;
        [SerializeField] private InventoryManager inventory;


        /// <summary>
        /// Debugs the current instance and finds auto-references.
        /// </summary>
        private void Awake()
        {
            Debug.Log($"[TradingUI] Initializing instance: {gameObject.name}");
            TryAutoFindRowsParent();
            TryAutoFindHeader();
        }

        /// <summary>
        /// Clears and initializes the UI on load.
        /// </summary>
        private void Start()
        {
            // Rebuild is called here primarily to ensure manager references are grabbed early.
            Rebuild();
        }

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
            
            Debug.Log($"[TradingUI] Attempting to set town to: {newTown.townName}");

            if (ReferenceEquals(newTown, town))
            { 
                RefreshAll(); 
                return; 
            }

            town = newTown;
            Debug.Log($"[TradingUI] Town successfully switched to: {town.townName}");
            
            TryAutoFindRowsParent();
            TryAutoFindHeader();
            UpdateHeaderUI();
            Rebuild();
        }

        /// <summary>
        /// Re-pulls prices/quantities on the existing rows (no re-instantiation).
        /// </summary>
        public void RefreshAll()
        {
            if (!EnsureManagers()) return;

            for (int i = 0; i < liveRows.Count; i++)
            {
                if (liveRows[i] != null) liveRows[i].Refresh();
            }
            UpdateHeaderUI();
        }

        /// <summary>
        /// Clears and rebuilds rows from the current town’s stock.
        /// </summary>
        public void Rebuild()
        {
            if (!EnsureManagers() || !EnsurePrefab() || !rowsParent)
            {
                ClearChildren(rowsParent);
                liveRows.Clear();
                UpdateHeaderUI();
                return;
            }

            ClearChildren(rowsParent);
            liveRows.Clear();
            UpdateHeaderUI();

            if (town == null || town.market == null || town.market.Count == 0) return;

            foreach (var entry in town.market)
            {
                if (entry == null || entry.item == null || entry.itemEconomy == null) continue;

                GameObject rowGO = Instantiate(rowPrefab, rowsParent);
                TradingItemRowUI row = rowGO.GetComponent<TradingItemRowUI>();
                
                if (row == null)
                {
                    Debug.LogError($"[TradingUI] Instantiated prefab '{rowPrefab.name}' is missing TradingItemRowUI component! Check the prefab itself.");
                    Object.Destroy(rowGO);
                    continue;
                }

                var le  = rowGO.GetComponent<LayoutElement>() ?? rowGO.gameObject.AddComponent<LayoutElement>();
                if (le.preferredHeight <= 0f) { le.preferredHeight = defaultRowHeight; le.minHeight = defaultRowHeight; }

                row.init(panel: this, trading: trading, inventory: inventory, town: town, item: entry.item);
                liveRows.Add(row);
            }
        }

        /// <summary>
        /// Ensures managers are available via serialized field, singleton, or scene lookup.
        /// </summary>
        private bool EnsureManagers()
        {
            if (trading == null)
            {
                trading = TradingManager.Instance;
                if (trading == null) trading = FindFirstObjectByType<TradingManager>();
            }
            
            if (inventory == null)
            {
                inventory = InventoryManager.Instance;
                if (inventory == null) inventory = FindFirstObjectByType<InventoryManager>();
            }
            
            if (trading == null || inventory == null)
            {
                if (logMissingRefs) Debug.LogError("[TradingUI] CRITICAL: TradingManager or InventoryManager not found.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the rowPrefab reference is valid.
        /// </summary>
        private bool EnsurePrefab()
        {
            if (rowPrefab) return true;

            if (logMissingRefs) Debug.LogError($"[TradingUI] CRITICAL: rowPrefab is NULL on instance '{gameObject.name}'. Assign the TradingRow prefab to the component.");
            return false;
        }


        /// <summary>
        /// Updates the header label based on the current town, preventing duplication.
        /// </summary>
        private void UpdateHeaderUI()
        {
            if (!headerTitleText) return; 

            var name = town ? town.townName : "Market";
            
            if (town != null)
            {
                headerTitleText.text = string.Format(headerFormat, name);
            }
            else
            {
                headerTitleText.text = name;
            }
        }

        /// <summary>
        /// Finds a ScrollRect content for rows if not assigned via known hierarchy paths.
        /// </summary>
        private void TryAutoFindRowsParent()
        {
            if (rowsParent) return;

            Transform content =
                transform.Find("RightColumn/MarketScrollView/Viewport/Content") ??
                transform.Find("LeftColumn/MarketScrollView/Viewport/Content") ??
                transform.Find("MarketScrollView/Viewport/Content") ??
                transform.Find("Viewport/Content");

            if (!content)
            {
                var scroll = GetComponentInChildren<ScrollRect>(true);
                if (scroll && scroll.content) content = scroll.content.transform;
            }

            rowsParent = content ? content.GetComponent<RectTransform>() : null;
            if (!rowsParent && logMissingRefs) Debug.LogWarning("[TradingUI] Could not locate rows parent. Assign it in the inspector.");
        }

        /// <summary>
        /// Binds header labels via known hierarchy paths.
        /// </summary>
        private void TryAutoFindHeader()
        {
            if (headerTitleText) return;

            var t =
                transform.Find("RightColumn/Header/Title") ??
                transform.Find("LeftColumn/Header/Title") ??
                transform.Find("Header/Title");

            headerTitleText = t ? t.GetComponent<TMP_Text>() : null;
            if (!headerTitleText && logMissingRefs) Debug.LogWarning("[TradingUI] Could not locate header title text. Drag it into the inspector.");
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
    }
}