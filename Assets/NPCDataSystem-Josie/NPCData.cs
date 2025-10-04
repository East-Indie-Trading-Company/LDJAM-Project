using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Objects/NPCData")]

public enum GreetingFlag
{
    None,
    Friendly,
    Neutral,
    Hostile
}
public class NPCData : ScriptableObject
{
    [SerializeField] string npcName;
    [SerializeField] Texture2D npcPotraitID;
    
    //DialogueConversation - vars : hasPlayed, ; methods : Trigger()
    [SerializeField] DialogueConversation[] npcDialogueLines; 
    [SerializeField] DialogueConversation[] npcRumorDialogueLines; 
    
    
    [SerializeField] Dictionary<GreetingFlag, string> npcGreetingDialogueLines;
    
    privat
    
    // insert Reputation Impact, whatever that is here
    [SerializeField] private int npcReputationImpact;
    
    void Talk()
    {
        // Check FLag, check priority of dialogue lines, then output Dialogue
        Array.ForEach(npcDialogueLines, line => {
                    if (!line.hasPlayed)
                    {
                        if (line.hasFlag)
                        {
                            //Check Priority of Dialogue Lines
                            // Trigger Dialogue System to start conversation with this NPC
                            line.hasPlayed = true; // Set hasPlayed to true after triggering
                        }
                        return;
                    }
                }
            );
    }
    
    void Rumor()
    {
       Array.ForEach(npcRumorDialogueLines, line => {
                    if (!line.hasPlayed)
                    {
                        if (line.hasFlag)
                        {
                            //Check Priority of Dialogue Lines
                            // Trigger Dialogue System to start conversation with this NPC
                            line.hasPlayed = true; // Set hasPlayed to true after triggering

                        }
                        return;
                    }
                }
            );
    }

    void Greeting()
    { 
        Array.ForEach(npcGreetingDialogueLines, greet => {
                    // Trigger Dialogue System to start conversation with this NPC

                    switch (greet.Key)
                    {
                        case GreetingFlag.Friendly:
                            // Friendly Greeting Logic
                            break;
                        case GreetingFlag.Neutral:
                            // Neutral Greeting Logic
                            break;
                        case GreetingFlag.Hostile:
                            // Hostile Greeting Logic
                            break;
                        default:
                            // Default Greeting Logic
                            break;
                    }

            }
            );
    }

}
