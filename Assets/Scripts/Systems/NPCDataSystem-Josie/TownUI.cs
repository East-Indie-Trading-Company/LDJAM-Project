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
        npcUI.DisplayNPCUI(npcData);
        tradingUI.SetTown(npcData.townStock);
        
    }

    
}
