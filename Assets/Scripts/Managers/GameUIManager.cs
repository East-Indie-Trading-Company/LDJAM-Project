using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private UIHoverScale[] townButtons;

    private void OnEnable()
    {
        foreach (var button in townButtons)
        {
            button.OnTownSelected += HandleTownSelected;
        }
    }

    private void OnDisable()
    {
        foreach (var button in townButtons)
        {
            button.OnTownSelected -= HandleTownSelected;
        }
    }

    private void HandleTownSelected(object sender, TownSelectedEventArgs e)
    {
        Debug.Log($"Town selected: {e.TownName} (ID {e.TownID}) at {e.ClickPosition}");
        // Pass this info to dialogue manager, map loader, etc.
        dialogueCanvas.SetActive(true);
    }

    public void HideDialogueCanvas()
    {
        dialogueCanvas.SetActive(false);
    }
}
