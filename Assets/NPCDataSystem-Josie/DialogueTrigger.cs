using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC DATA")]
    public NPCData npcData = null;
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI npcNameText; // To be implemented
    [SerializeField] Image npcPortrait;
    [SerializeField] TextMeshProUGUI townNameText; // To be implemented
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] Button chatButton;
    [SerializeField] Button rumorButton;
    [SerializeField] GameObject marketCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chatButton.onClick.AddListener(onChat);
        rumorButton.onClick.AddListener(onRumor);
        DisplayTown(); // VERY SUPER TEMOIRARY
    }

    void DisplayTown()
    {
        if (npcData != null)
        {
            npcPortrait.sprite = npcData.displayInfo.npcIcon;
            npcData.Greeting(npcText);
        }
        else
        {
            Debug.LogWarning("[DialogueTrigger] NPC data is missing!");
        }
    }

    void onChat()
    {
        marketCanvas.SetActive(false);
        npcData.Talk();
    }
    void onRumor()
    {
        marketCanvas.SetActive(false);
        npcData.Rumor();
    }
}
