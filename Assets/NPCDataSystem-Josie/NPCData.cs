using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Object/NPCData")]


public class NPCData : ScriptableObject
{
    
    [SerializeField] public GameObject objPrefab; 
    [SerializeField] string npcName;
    [SerializeField] Texture2D npcPotraitID;
    
    //DialogueConversation - vars : hasPlayed, ; methods : Trigger()
    [SerializeField] DialogueConversation[] npcDialogueLines; 
    [SerializeField] DialogueConversation[] npcRumorDialogueLines; 
    
    //
    [SerializeField] string[] npcGreetingDialogueLines;
    
    
    // insert Reputation Impact, whatever that is here
    [SerializeField] private int npcReputationImpact;
    
    void Talk()
    {
        // Check FLag, check priority of dialogue lines, then output Dialogue
        Array.ForEach(npcDialogueLines, line => {
                    if (!line.hasPlayed)
                    {
                        bool flagsTrue = true;
                        foreach (string flag in line.flags)
                        {
                            // If any flag is not true, set false
                            if (!FlagManager.Instance.GetFlag(flag))
                            {
                                flagsTrue = false;
                            }
                        }
                        if (flagsTrue)
                        {
                            //Check Priority of Dialogue Lines
                            // Trigger Dialogue System to start conversation with this NPC
                            DialogueManager.Instance.StartDialogue(line);
                            // line.hasPlayed = true; // Set hasPlayed to true after triggering /////////////// You won't need this line. Dialogue manager will handle it
                            return;
                        }        

                    }
                }
            );
    }
    
    void Rumor()
    {
       Array.ForEach(npcRumorDialogueLines, line => {
                    if (!line.hasPlayed)
                    {
                        bool flagsTrue = true;
                        foreach (string flag in line.flags)
                        {
                            // If any flag is not true, set false
                            if (!FlagManager.Instance.GetFlag(flag))
                            {
                                flagsTrue = false;
                            }
                        }
                        if (flagsTrue)
                        {
                            //Check Priority of Dialogue Lines
                            // Trigger Dialogue System to start conversation with this NPC
                            DialogueManager.Instance.StartDialogue(line);
                            // line.hasPlayed = true; // Set hasPlayed to true after triggering /////////////// You won't need this line. Dialogue manager will handle it
                            return;
                        }    
                    }
                }
            );
    }

    void Greeting()
    { 
        Array.ForEach(npcGreetingDialogueLines, greet => {
            // Trigger Dialogue System to start conversation with this NPC
            return;
                    //check flag or something

            }
            );
    }

}
