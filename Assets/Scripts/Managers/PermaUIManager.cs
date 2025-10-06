using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermaUIManager: MonoBehaviour
{
    public static PermaUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI dayTrackerUI;
    [SerializeField] private TextMeshProUGUI daysRemainingUI;
    [SerializeField] private Image reputationBar;
    [SerializeField] private Image reputationSlider;

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

    public void SetCurrencyUI( int newCurrency )
    {
        Debug.Log($"[PermaUI] Currency Updated to: {newCurrency}");
        currencyUI.text = newCurrency.ToString();
    }

    public void SetDayTrackerUI(int newDays)
    {
        currencyUI.text = "Day " + newDays.ToString();
    }

    public void SetDaysRemainingUI(int newDaysRemaining)
    {
        currencyUI.text = newDaysRemaining.ToString() + " Days Remain";
    }

    public void SetReputationUI(float newReputation)
    {
        float halfWidth = reputationBar.rectTransform.rect.width / 2f;
        newReputation = Mathf.Clamp(newReputation, -1f, 1f);
        float newXPosition = halfWidth * newReputation + halfWidth;

        Vector3 newSliderPosition = reputationSlider.rectTransform.anchoredPosition;
        newSliderPosition.x = newXPosition;

        reputationSlider.rectTransform.anchoredPosition = newSliderPosition;
    }
}
