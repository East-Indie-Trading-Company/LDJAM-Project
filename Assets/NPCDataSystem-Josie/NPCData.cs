using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Object/NPCData")]


public class NPCData : ScriptableObject
{

    [SerializeField] public GameObject objPrefab;
    public NpcDisplayInfo displayInfo; // This contains npc name, portrait, town name, and (not implemented town icon)

    //DialogueConversation - vars : hasPlayed, ; methods : Trigger()
    [SerializeField] DialogueConversation[] npcDialogueLines;
    [SerializeField] DialogueConversation[] npcRumorDialogueLines;

    //
    [SerializeField] DialogueConversation[] npcGreetingDialogueLines;


    // insert Reputation Impact, whatever that is here
    [SerializeField] private int npcReputationImpact;

    public void Talk()
    {
        // Check FLag, check priority of dialogue lines, then output Dialogue
        Array.ForEach(npcDialogueLines, line =>
        {
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

    public void Rumor()
    {
        Array.ForEach(npcRumorDialogueLines, line =>
        {
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

    public void Greeting(TextMeshProUGUI textComponent)
    {
        Array.ForEach(npcGreetingDialogueLines, greet =>
        {
            // Trigger Dialogue System to start conversation with this NPC
            textComponent.text = greet.lines[0].dialogueText;
            return;
            //check flag or something
            
        }
            );
    }



}
