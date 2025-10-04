using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentDay { get; private set; } = 0;

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

    private void Start()
    {
        // Initialize subsystems here later
    }

    public void AdvanceDay()
    {
        CurrentDay++;
        FlagManager.Instance?.OnDayAdvanced(CurrentDay);
        Debug.Log($"Day advanced to {CurrentDay}");
    }
}