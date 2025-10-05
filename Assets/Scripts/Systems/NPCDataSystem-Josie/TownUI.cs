using UnityEngine;
using TMPro;
public class TownUI : MonoBehaviour
{
    [Header("Town UI references")]
    [SerializeField] NPCUI npcUI;
    [SerializeField] Trading.TradingUI tradingUI;
    NPCData npcData = null;
    public NPCData testData;


    // THIS NEEDS TO BE CALLED WHEN A PLAYER CLICKS A TOWN
    public void SetTownUI(NPCData newData)
    {
        Debug.Log($"[TownUI] Update town ui to {newData.displayInfo.townName}");
        npcData = newData;
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

    [ContextMenu("Run UI Test")]
    public void TestSetUI()
    {
        SetTownUI(testData);
    }
    
}
