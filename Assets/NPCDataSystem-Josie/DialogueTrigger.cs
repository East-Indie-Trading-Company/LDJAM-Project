using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("NPC DATA")]
    public NPCData npcData = null;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] Image npcPortrait;
    [SerializeField] TextMeshProUGUI townNameText;
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] Button chatButton;
    [SerializeField] Button rumorButton;
    [SerializeField] GameObject marketCanvas;

    void Start()
    {
        chatButton.onClick.AddListener(onChat);
        rumorButton.onClick.AddListener(onRumor);
    }

    // Arwen this was Marco called by ZoomInTest when player clicks a town
    public void SetNPC(NPCData newData)
    {
        npcData = newData;
        DisplayTown();
    }

    void DisplayTown()
    {
        if (npcData == null)
        {
            Debug.LogWarning("[DialogueTrigger] npcData is NULL!");
            return;
        }

        if (npcData.displayInfo == null)
        {
            Debug.LogWarning($"[DialogueTrigger] displayInfo is NULL for {npcData.name}");
            return;
        }

        

        // ✅ Only update UI if everything exists
        if (npcPortrait != null)
            npcPortrait.sprite = npcData.displayInfo.npcIcon;

        if (npcNameText != null)
            npcNameText.text = npcData.displayInfo.npcName;

        if (townNameText != null)
            townNameText.text = npcData.displayInfo.townName;

        if (npcText != null)
            npcData.Greeting(npcText);
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