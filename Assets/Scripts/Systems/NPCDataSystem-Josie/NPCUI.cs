using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class NPCUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] Image npcPortrait;
    [SerializeField] TextMeshProUGUI townNameText;
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] Button chatButton;
    [SerializeField] Button rumorButton;
    [SerializeField] GameObject marketCanvas;

    NPCData npcData;

    void Start()
    {
        chatButton.onClick.AddListener(onChat);
        rumorButton.onClick.AddListener(onRumor);
    }
    public void onChat()
    {
        marketCanvas.SetActive(false);
        npcData.Talk();
    }

    public void onRumor()
    {
        marketCanvas.SetActive(false);
        npcData.Rumor();
    }
    // Set the npc ui to the values held in the npcData object
    public void DisplayNPCUI(NPCData newData)
    {
        marketCanvas.SetActive(true);
        npcData = newData;
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
}
