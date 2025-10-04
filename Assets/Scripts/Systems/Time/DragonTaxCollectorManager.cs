using UnityEngine;

public class DragonTaxCollectorManager : DayAdvanceListener
{
    [Header("Config")]
    [Tooltip("How many days between each tax collection.")]
    [SerializeField] private int taxInterval = 10;

    [Tooltip("The first day when the dragon starts collecting taxes.")]
    [SerializeField] private int firstCollectionDay = 10;

    private int lastTriggeredDay = -1;

    protected override void HandleDayAdvanced(int newDay)
    {
        if (newDay >= firstCollectionDay && (newDay - firstCollectionDay) % taxInterval == 0)
        {
            lastTriggeredDay = newDay;
            Debug.Log($"[DragonTaxCollector] Day {newDay}: Dragon arrives to collect taxes!");
            // TODO: trigger event/cutscene/UI
        }
    }

    public int GetNextCollectionDay(int currentDay)
    {
        if (currentDay < firstCollectionDay)
            return firstCollectionDay;

        int cycles = (currentDay - firstCollectionDay) / taxInterval + 1;
        return firstCollectionDay + cycles * taxInterval;
    }

    public int GetDaysUntilNextCollection(int currentDay)
    {
        int next = GetNextCollectionDay(currentDay);
        return Mathf.Max(0, next - currentDay);
    }
}
