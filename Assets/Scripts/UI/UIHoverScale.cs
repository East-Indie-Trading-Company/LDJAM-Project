using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    private Vector3 hoverScale = new Vector3(.5f, .5f, .5f);

    [Header("City Info")]
    [SerializeField] private string cityName;                 // nome da cidade deste botão
    [SerializeField] private TextMeshProUGUI cityNameText;    // referência ao texto do Canvas


    private void Awake()
    {
        originalScale = transform.localScale;

        if (cityNameText != null)
            cityNameText.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale + hoverScale;

        if (cityNameText != null)
        {
            cityNameText.text = cityName;
            cityNameText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;

        if (cityNameText != null)
            cityNameText.gameObject.SetActive(false);
    
}

        public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked mouse button");
        // clickSound?.Play(); // for later use
    }
}