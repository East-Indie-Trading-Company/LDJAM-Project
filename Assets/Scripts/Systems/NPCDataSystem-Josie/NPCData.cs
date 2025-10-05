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
    [SerializeField] DialogueConversation[] npcGreetingDialogueLines;


    // insert Reputation Impact, whatever that is here
    [SerializeField] private int npcReputationImpact;

    public void Talk()
    {
        DialogueManager.Instance.StartDialogue(PullDialogue(npcDialogueLines));
    }

    public void Rumor()
    {
        DialogueManager.Instance.StartDialogue(PullDialogue(npcRumorDialogueLines));
    }

    public void Greeting(TextMeshProUGUI textComponent)
    {
        DialogueConversation convo =  PullDialogue(npcGreetingDialogueLines);
        textComponent.text = convo.lines[0].dialogueText;
    }


    DialogueConversation PullDialogue(DialogueConversation[] conversations)
    {
        DialogueConversation convoToWrite = null;
        // TODO:: List/array of convos that don't need flags here
        Array.ForEach(conversations, convo =>
        {
            if (!convo.hasPlayed) // If the line is repeatable or has not played yet.
            {
                if (convo.flags[0] != null) // If the conversation has at least one requirement, check requirements.
                {
                    bool flagsTrue = true;
                    foreach (string flag in convo.flags) // Check all flags
                    {
                        if (!FlagManager.Instance.GetFlag(flag))
                        {
                            flagsTrue = false;
                        }
                    }
                    if (flagsTrue) // If all flags are true
                    {
                        convoToWrite = convo; // This is the convo to write
                        return;
                    }
                }
                else
                {
                    // TODO:: Create list that doesn't require flags
                }
            }

        });

        if (convoToWrite == null)
        {
            // TODO:: Randomly choose from a list that doesn't require flags
        }

        return convoToWrite;
    }


}
