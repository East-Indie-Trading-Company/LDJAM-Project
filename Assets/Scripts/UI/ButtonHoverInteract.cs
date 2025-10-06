using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    private Vector3 hoverScale = new Vector3(.1f, .1f, .1f);
    private AudioSource hoverSound;
    private AudioSource clickSound;

    private void Awake()
    {
        originalScale = transform.localScale;

        GameObject hoverObj = GameObject.Find("hoverSound");
        if (hoverObj != null)
            hoverSound = hoverObj.GetComponent<AudioSource>();

        GameObject clickObj = GameObject.Find("clickSound");
        if (clickObj != null)
            clickSound = clickObj.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale + hoverScale;
        Debug.Log("Hover sound played");
        hoverSound?.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickSound?.Play();
    }
}