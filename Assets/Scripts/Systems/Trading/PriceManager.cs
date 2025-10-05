using UnityEngine;

namespace Trading
{
    public static class PriceCalculator
    {

        /// <summary>
        /// Calculates the price at which a town will buy an item for the player.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="playerReputation"></param>
        /// <param name="inflationIndex"></param>
        /// <returns></returns>
        public static int GetBuyPriceForPlayer(
            TownStock.MarketEntry entry,
            float inflationIndex)
        {
            if (entry == null || entry.itemEconomy == null)
                return 0;

            var itemTownConfig = entry.itemEconomy;
            int baseBuy = Mathf.Max(1, itemTownConfig.buyPrice);

            float multiplier = itemTownConfig.GetDynamicPriceMultiplier(entry.stock);
            float priceFinal = baseBuy * multiplier * Mathf.Max(1f, inflationIndex);

            return Mathf.Max(0, Mathf.RoundToInt(priceFinal));
        }


        /// <summary>
        /// Calculates the price at which a town will sell an item to the player.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="playerReputation"></param>
        /// <param name="inflationIndex"></param>
        /// <returns></returns>
        public static int GetSellPriceToPlayer(
            TownStock.MarketEntry entry,
            float inflationIndex)
        {
            if (entry == null || entry.itemEconomy == null)
                return 0;

            var itemTownConfig = entry.itemEconomy;
            int baseSell = Mathf.Max(1, itemTownConfig.sellPrice);

            float multiplier = itemTownConfig.GetDynamicPriceMultiplier(entry.stock);
            float inflationMult = Mathf.Max(1f, inflationIndex);

            float priceFinal = baseSell * multiplier * inflationMult;
            return Mathf.Max(0, Mathf.RoundToInt(priceFinal));
        }
    }
}
