using UnityEngine;
using TMPro;
public class TownUI : MonoBehaviour
{
    [Header("Town UI references")]
    [SerializeField] NPCUI npcUI;
    [SerializeField] Trading.TradingUI tradingUI;
    NPCData npcData = null;


    // THIS NEEDS TO BE CALLED WHEN A PLAYER CLICKS A TOWN
    public void SetTownUI(NPCData newData)
    {
        npcData = newData;
        //Debug.Log($"[TownUI] Update town ui to {newData.displayInfo.townName}");
        if (npcData != null)
        {
            npcUI.DisplayNPCUI(npcData);
        }
        else
        {
            Debug.LogWarning($"[TownUI] NPCData not assigned!");
        }

        if (npcData.townStock != null)
        {
            tradingUI.SetTown(npcData.townStock);
        }
        else
        {
            Debug.LogWarning($"[TownUI] TownStock not assigned!");
        }

    }

    public void RemoveTownUI()
    {
        npcUI.CloseUI();
    }
    
}
