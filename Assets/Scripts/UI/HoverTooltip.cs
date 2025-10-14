using UnityEngine;
using TMPro;

public class HoverTooltip : MonoBehaviour
{
    public static HoverTooltip Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    private RectTransform rectTransform;
    private bool isVisible;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        HideInstant();
    }

    private void Update()
    {
        if (!isVisible) return;

        // Move tooltip with the cursor
        rectTransform.position = Input.mousePosition + (Vector3)offset;
    }

    public void Show(string cityName)
    {
        tooltipText.text = cityName;
        isVisible = true;
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        isVisible = false;
        canvasGroup.alpha = 0f;
    }

    private void HideInstant()
    {
        isVisible = false;
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}
