using UnityEngine;
using UnityEngine.Events;
using Trading;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentDay { get; private set; } = 1;

    //This listener is called by inventory manager any time our player currency changes
    //And can be used for any effects you want from a currency change, such as SFX
    //Passes new gold amount
    public UnityEvent<int> onCurrencyChanged;

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

    public void OnEnable()
    {
        onCurrencyChanged.AddListener(OnCurrencyChanged);
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

    public void OnCurrencyChanged(int newCurrencyAmount)
    {

    }
}
