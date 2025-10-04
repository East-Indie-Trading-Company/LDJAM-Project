using UnityEngine;
using Trading;

namespace Trading.Tests
{
    // Attach to an empty GameObject in a test scene.
    // No UI: use the Inspector's gear/⋮ menu to run actions.
    public class EconomySmokeTest : MonoBehaviour
    {
        [Header("Refs (assign in Inspector)")]
        [SerializeField] private EconomyManager economy; // optional: auto-grab Instance
        [SerializeField] private TownStock town;         // REQUIRED: the town asset to inspect

        [Header("Town Buyback Margin (for SELL price calc)")]
        [Range(0.1f, 1f)] public float townSellMargin = 0.85f;

        private void EnsureRefs()
        {
            if (!economy) economy = EconomyManager.Instance;
        }

        [ContextMenu("Log Current Prices")]
        public void LogCurrentPrices()
        {
            EnsureRefs();
            if (!town) { Debug.LogError("EconomySmokeTest: TownStock not assigned."); return; }

            float infl = economy ? economy.InflationIndex : 1f;
            Debug.Log($"[ECON] Town={town.townName}  Inflation={infl:F4}");

            foreach (var e in town.market)
            {
                if (e.item == null || e.itemEconomy == null) continue;

                int buy  = PriceCalculator.GetBuyPriceForPlayer(e, infl);
                int sell = PriceCalculator.GetSellPriceToPlayer(e, infl);

                Debug.Log(
                    $"{e.item.itemName} | stock={e.stock} " +
                    $"baseBuy={e.itemEconomy.buyPrice} baseSell={e.itemEconomy.sellPrice} " +
                    $"→ BUY={buy} SELL={sell}");
            }
        }

        [ContextMenu("Advance Day + Log Prices")]
        public void AdvanceDayAndLog()
        {
            EnsureRefs();
            economy?.AdvanceDay();
            LogCurrentPrices();
        }
    }
}
