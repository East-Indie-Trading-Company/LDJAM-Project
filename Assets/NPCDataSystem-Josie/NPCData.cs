using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Objects/NPCData")]


public class NPCData : ScriptableObject
{
    
    [SerializeField] public GameObject objPrefab; 
    [SerializeField] string npcName;
    [SerializeField] Texture2D npcPotraitID;
    
    //DialogueConversation - vars : hasPlayed, ; methods : Trigger()
    [SerializeField] DialogueConversation[] npcDialogueLines; 
    [SerializeField] DialogueConversation[] npcRumorDialogueLines; 
    
    //
    [SerializeField] List<string> npcGreetingDialogueLines;
    
    
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
                
                    //check flag or something

            }
            );
    }

}
