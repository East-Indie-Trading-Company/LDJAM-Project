using UnityEngine;

namespace Trading
{
    public static class PriceCalculator
    {

        /// <summary>
        /// Calculates the price at which a town will sell an item to the player.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="playerReputation"></param>
        /// <param name="inflationIndex"></param>
        /// <returns></returns>
        public static int GetBuyPriceForPlayer(
            TownStock.MarketEntry entry,
            float playerReputation,
            float inflationIndex)
        {
            if (entry == null || entry.item == null) return 0;
            var basePrice = entry.EffectiveBasePrice;
            var itemConfig = entry.itemEconomy;

            float stockMult = 1f;
            float repMult = 1f;

            if (itemConfig != null)
            {
                var normStock = Mathf.Clamp01(entry.stock / 100f); // same NormalizeStock inline
                stockMult = Mathf.Max(0f, itemConfig.stockToPriceMultiplier.Evaluate(normStock));
                repMult   = Mathf.Max(0f, itemConfig.reputationToPriceMultiplier.Evaluate(Mathf.Clamp(playerReputation, -1f, 1f)));
            }

            var priceF = basePrice * stockMult * repMult * Mathf.Max(1f, inflationIndex);
            return Mathf.Max(0, Mathf.RoundToInt(priceF));
        }

        /// <summary>
        /// Calculates the price at which a town will buy an item from the player.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="playerReputation"></param>
        /// <param name="inflationIndex"></param>
        /// <param name="tuning"></param>
        /// <returns></returns>
        public static int GetSellPriceToPlayer(
            TownStock.MarketEntry entry,
            float playerReputation,
            float inflationIndex,
            float townSellMargin = 0.85f)  // designer-provided later; default OK for test
        {
            var buy = GetBuyPriceForPlayer(entry, playerReputation, inflationIndex);
            var margin = Mathf.Clamp01(townSellMargin);
            return Mathf.RoundToInt(buy * margin);
        }

        /// <summary>
        /// Normalizes stock to a 0..1 range for price multiplier evaluation.
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>

        private static float NormalizeStock(int stock)
        {
            return Mathf.Clamp01(stock / 100f);
        }
    }
}
