using System.Collections.Generic;
using UnityEngine;

namespace Trading
{
    public class EconomyManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the EconomyManager.
        /// </summary>
        public static EconomyManager Instance { get; private set; }

        [Header("Towns")]
        [SerializeField] private List<TownStock> towns = new();

        [Header("Inflation")]
        [Tooltip("Slow global rise in prices over time (applied multiplicatively).")]
        [SerializeField, Range(0f, 0.02f)] private float dailyInflationRate = 0.001f; //changeable rate once we get this balanced
        [SerializeField] private float inflationIndex = 1f; // inflation starts at 1 and never stops increasing

        public float InflationIndex => inflationIndex;

        /// <summary>
        /// Ensure a single instance of the EconomyManager exists.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Registers a town with the economy manager.
        /// </summary>
        /// <param name="town"></param>
        public void RegisterTown(TownStock town)
        {
            if (town != null && !towns.Contains(town)) towns.Add(town);
        }

        /// <summary>
        /// Advances the economy by one day, updating stock levels and applying inflation.
        /// </summary>
        public void AdvanceDay()
        {
            foreach (var town in towns)
            {
                foreach (var entry in town.market)
                {
                    if (entry.item == null) continue;
                    var townItemConfig = entry.itemEconomy;
                    if (townItemConfig == null) continue;

                    int delta = townItemConfig.GetDailyStockDelta();
                    entry.stock = Mathf.Max(0, entry.stock + delta);
                }
            }

            inflationIndex *= (1f + dailyInflationRate);
        }
    }
}
