using UnityEngine;
using UnityEngine.Events;
using Trading;
using System.Reflection;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int campaignMaxDays = 30;   // total days per cycle
    private int daysRemaining;

    public static GameManager Instance { get; private set; }

    public int CurrentDay { get; private set; } = 1;

    [Header("World State")]
    public TownData currentTown;

    [Header("Managers")]
    public FlagManager flagManager;
    public EconomyManager economyManager;
    public PermaUIManager permaUIManager;
    public InventoryManager inventoryManager;

    [Header("Events")]
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

    private void Start()
    {
        daysRemaining = campaignMaxDays - CurrentDay;

        // Initialize UI if PermaUIManager exists
        if (permaUIManager != null)
        {
            permaUIManager.SetCurrencyUI(InventoryManager.Instance.Gold);
            UpdatePermaUIHUD();
        }
    }

    private void OnEnable()
    {
        onCurrencyChanged.AddListener(OnCurrencyChanged);
    }

    private void OnDisable()
    {
        onCurrencyChanged.RemoveListener(OnCurrencyChanged);
    }

    // --- TIME MANAGEMENT ---

    public void AdvanceDay()
    {
        CurrentDay++;
        daysRemaining--;

        // ✅ Reset cycle if we reach 0 days remaining
        if (daysRemaining <= 0)
        {
            daysRemaining = campaignMaxDays;
            CurrentDay = 1; // Optional: reset day count too
            Debug.Log("[GameManager] Cycle reset! Starting a new period.");
        }

        flagManager?.TriggerDayAdvanced(CurrentDay);
        economyManager?.AdvanceDay();

        UpdatePermaUIHUD();

        Debug.Log($"[GameManager] Day advanced to {CurrentDay}");
    }

    public void AdvanceDays(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AdvanceDay();
        }
    }

    // --- ECONOMY ---

    public void OnCurrencyChanged(int newCurrencyAmount)
    {
        permaUIManager.SetCurrencyUI(InventoryManager.Instance.Gold);
    }

    // --- WORLD LOGIC ---

    public void EnterTown(TownData town)
    {
        currentTown = town;
        Debug.Log($"[GameManager] Entered town: {town.townName}");
    }

    public void TravelToTown(TownData newTown)
    {
        // If no previous town (first visit)
        if (currentTown == null)
        {
            EnterTown(newTown);
            AdvanceDay();
            return;
        }

        float distance = Vector3.Distance(currentTown.transform.position, newTown.transform.position);

        // Convert distance to travel days
        int daysPassed = Mathf.Max(1, Mathf.CeilToInt(distance * 0.2f)); // tweak factor

        AdvanceDays(daysPassed);
        EnterTown(newTown);

        Debug.Log($"[GameManager] Traveled from {currentTown.townName} to {newTown.townName}. Distance: {distance:F1}, Days passed: {daysPassed}");
    }

    public void ReturnToMap()
    {
        Debug.Log("[GameManager] Returned to world map view");
        currentTown = null;
    }

    // --- PERMA UI UPDATE ---

    private void UpdatePermaUIHUD()
    {
        var perma = PermaUIManager.Instance;
        if (perma == null) return;

        var t = perma.GetType();
        var dayField = t.GetField("dayTrackerUI", BindingFlags.NonPublic | BindingFlags.Instance);
        var daysRemField = t.GetField("daysRemainingUI", BindingFlags.NonPublic | BindingFlags.Instance);

        var dayTMP = dayField?.GetValue(perma) as TextMeshProUGUI;
        var daysRemTMP = daysRemField?.GetValue(perma) as TextMeshProUGUI;

        if (dayTMP != null)
            dayTMP.text = $"Day {CurrentDay}";

        if (daysRemTMP != null)
            daysRemTMP.text = $"{daysRemaining} Days Remain";
    }
}
