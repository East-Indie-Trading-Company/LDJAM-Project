using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    private Vector3 hoverScale = new Vector3(.1f, .1f, .1f);

    [Header("City Info")]
    [SerializeField] private string cityName;  // Nome da cidade a mostrar no tooltip

    [Header("Audio (Opcional)")]
    [SerializeField] private AudioSource hoverSound;
    [SerializeField] private AudioSource clickSound;

    private void Awake()
    {
        originalScale = transform.localScale;

        // fallback: tenta encontrar objetos de som globais se não estiverem ligados
        if (hoverSound == null)
        {
            GameObject h = GameObject.Find("hoverSound");
            if (h != null) hoverSound = h.GetComponent<AudioSource>();
        }

        if (clickSound == null)
        {
            GameObject c = GameObject.Find("clickSound");
            if (c != null) clickSound = c.GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale + hoverScale;
        hoverSound?.Play();
        HoverTooltip.Instance?.Show(cityName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        HoverTooltip.Instance?.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickSound?.Play();
        HoverTooltip.Instance?.Hide();
        Debug.Log($"Clicked on {cityName}");
    }
}
