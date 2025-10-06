using TMPro;
using UnityEngine;

public class TimeHUD : DayAdvanceListener
{
    [Header("UI References")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text dragonCountdownText;

    [Header("Refs")]
    [SerializeField] private DragonTaxCollectorManager taxCollector;

    protected override void HandleDayAdvanced(int newDay)
    {
        RefreshUI(newDay);
    }

    protected override void Start()
    {
        base.Start(); // <- IMPORTANTE: chama o Start da classe base DayAdvanceListener
        RefreshUI(GameManager.Instance.CurrentDay);
    }

    private void RefreshUI(int currentDay)
    {
        Debug.Log($"[TimeHUD] RefreshUI called for Day {currentDay}");

        if (dayText != null)
            dayText.text = $"Day {currentDay}";
        else
            Debug.LogWarning("[TimeHUD] dayText is NULL!");

        if (dragonCountdownText != null && taxCollector != null)
        {
            int daysLeft = taxCollector.GetDaysUntilNextCollection(currentDay);

            if (daysLeft > 0)
                dragonCountdownText.text = $"{daysLeft} days until the Dragon arrives";
            else
                dragonCountdownText.text = "The Dragon is here!";
        }
        else
        {
            if (dragonCountdownText == null) Debug.LogWarning("[TimeHUD] dragonCountdownText is NULL!");
            if (taxCollector == null) Debug.LogWarning("[TimeHUD] taxCollector is NULL!");
        }
    }
}
