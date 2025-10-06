using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PermaUIManager: MonoBehaviour
{
    public static PermaUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI dayTrackerUI;
    [SerializeField] private TextMeshProUGUI daysRemainingUI;
    [SerializeField] private Image reputationBar;
    [SerializeField] private Image reputationSlider;
    [SerializeField] private GameObject permaUICanvas;

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

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "Map")
        {
            permaUICanvas.SetActive(true);
        }
    }

    public void SetCurrencyUI(int newCurrency)
    {
        Debug.Log($"[PermaUI] Currency Updated to: {newCurrency}");
        currencyUI.text = newCurrency.ToString();
    }

    public void SetDayTrackerUI(int newDays)
    {
        dayTrackerUI.text = "Day " + newDays.ToString();
    }

    public void SetDaysRemainingUI(int newDaysRemaining)
    {
        daysRemainingUI.text = newDaysRemaining.ToString() + " Days Remain";
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
