using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Object/NPCData")]


public class NPCData : ScriptableObject
{
    [Header("References")]
    public NpcDisplayInfo displayInfo; // This contains npc name, portrait, town name, and (not implemented town icon)
    public Trading.TownStock townStock;

    [Header("Dialogue Pools")]
    [SerializeField] DialogueConversation[] npcDialogueLines;
    [SerializeField] DialogueConversation[] npcRumorDialogueLines;
    [SerializeField] DialogueConversation[] npcGreetingDialogueLines;

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

        if (convo.lines.Count > 0)
        {
            textComponent.text = convo.lines[0].dialogueText;
        }
        else
        {
            Debug.Log($"[NPCData] NO LINES!");
        }
    }


    DialogueConversation PullDialogue(DialogueConversation[] conversations)
    {
        //Debug.Log($"[NPCData] Num of convos in this group: {conversations.Length}");
        DialogueConversation convoToWrite = null;
        // TODO:: List/array of convos that don't need flags here
        List<DialogueConversation> convosWithNoFlags = new List<DialogueConversation>();
        Array.ForEach(conversations, convo =>
        {
            if (!convo.hasPlayed) // If the line is repeatable or has not played yet.
            {
                if (convo.flags.Length > 0) // If the conversation has at least one requirement, check requirements.
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
                    // Add to list that doesn't require flags
                    convosWithNoFlags.Add(convo);
                }
            }

        });

        if (convoToWrite == null) 
        {
            //Debug.Log($"[NPCData] Num of convos with no flags: {convosWithNoFlags.Count}");
            // Randomly choose from a list that doesn't require flags
            if (convosWithNoFlags.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, convosWithNoFlags.Count-1);
                //Debug.Log($"[NPCData] Random Index: {randomIndex}");
                convoToWrite = convosWithNoFlags[randomIndex];
            }
            else
            {
                Debug.LogWarning("[NPCData] NO VIABLE CONVERSATIONS"); ///////////   IF NO CONVERSATIONS ARE VIABLE, THIS WILL CAUSE PROBLEMS
            }
            
        }

        return convoToWrite;
    }


}
