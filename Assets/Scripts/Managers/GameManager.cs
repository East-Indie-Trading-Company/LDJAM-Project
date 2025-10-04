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
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceDay()
    {
        CurrentDay++;
        FlagManager.Instance?.TriggerDayAdvanced(CurrentDay);
        
    }

    public void AdvanceDays(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AdvanceDay();
        }
    }
}
