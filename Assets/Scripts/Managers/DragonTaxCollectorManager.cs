using UnityEngine;

public class DragonTaxCollectorManager : DayAdvanceListener
{
    [Header("Config")]
    [Tooltip("How many days between each tax collection.")]
    [SerializeField] private int taxInterval = 10;

    [Tooltip("The first day when the dragon starts collecting taxes.")]
    [SerializeField] private int firstCollectionDay = 10;

    protected override void HandleDayAdvanced(int newDay)
    {
        // The dragon appears on the firstCollectionDay and then every taxInterval days
        if (newDay >= firstCollectionDay && (newDay - firstCollectionDay) % taxInterval == 0)
        {
            Debug.Log($"[DragonTaxCollector] Day {newDay}: Dragon arrives to collect taxes!");

            // TODO: Implement real logic here:
            // - Open tax collection UI
            // - Check player's gold
            // - Apply penalties if the player cannot pay
            // - Trigger dragon dialogue/cutscene
        }
    }
}