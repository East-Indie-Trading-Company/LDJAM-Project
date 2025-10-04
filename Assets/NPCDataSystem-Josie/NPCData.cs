using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Objects/NPCData")]
public class NPCData : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private Texture2D npcPotraitID;
    
    [SerializeField] private string[] npcDialogueLines; // might change to a Dictionary later
    [SerializeField] private string[] npcRumorDialogueLines; // might change to a Dictionary later
    
    // insert Reputation Impact, whatever that is here
    [SerializeField] private int npcReputationImpact;
}
