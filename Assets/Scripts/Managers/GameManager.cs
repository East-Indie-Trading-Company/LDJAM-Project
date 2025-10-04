using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentDay { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        this.instance.EconomyManager = GetComponent<EconomyManager>();
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceDay()
    {
        CurrentDay++;
        FlagManager.Instance?.TriggerDayAdvanced(CurrentDay);
        EconomyManager.Instance?.AdvanceDay();
        Debug.Log($"Day advanced to {CurrentDay}");
    }

    public void AdvanceDays(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EconomyManager.Instance?.AdvanceDay();
            AdvanceDay();
        }
    }
}
