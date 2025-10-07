using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TownSelectedEventArgs : EventArgs
{
    public int TownID { get; }
    public string TownName { get; }
    public Vector3 ClickPosition { get; }

    public TownSelectedEventArgs(int townID, string townName, Vector3 clickPosition)
    {
        TownID = townID;
        TownName = townName;
        ClickPosition = clickPosition;
    }
}

public class UIHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private int townID;
    [SerializeField] private string townName;

    public event EventHandler<TownSelectedEventArgs> OnTownSelected;

    private Vector3 originalScale;
    private Vector3 hoverScale = new Vector3(.5f, .5f, .5f);

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale + hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked mouse button");
        // clickSound?.Play(); // for later use

        Debug.Log($"Clicked on town {townName} (ID {townID})");

        var args = new TownSelectedEventArgs(
            townID,
            townName,
            eventData.pointerPressRaycast.worldPosition
        );

        OnTownSelected?.Invoke(this, args);
    }
}