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
        Debug.Log($"Pull {displayInfo.npcName} chat pool");
        DialogueManager.Instance.StartDialogue(PullDialogue(npcDialogueLines));
    }

    public void Rumor()
    {
        Debug.Log($"Pull {displayInfo.npcName} rumor pool");
        DialogueManager.Instance.StartDialogue(PullDialogue(npcRumorDialogueLines));
    }

    public void Greeting(TextMeshProUGUI textComponent)
    {
        Debug.Log($"Pull {displayInfo.npcName} greeting pool");
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
        
        List<DialogueConversation> validConvos = new List<DialogueConversation>();
        List<DialogueConversation> highPriorityConvos = new List<DialogueConversation>();

        if (conversations.Length == 0)
        {
            Debug.LogWarning("NO CONVERSATIONS IN THIS POOL {displayInfo.npcName}");
        }
        Array.ForEach(conversations, convo =>
        {
            if (convo == null) Debug.LogWarning("THIS CONVO HAS NOT BEEN ASSIGNED");

            if (!convo.hasPlayed) // If the line is repeatable or has not played yet.
            {
                if (convo.flags.Length > 0) // If the conversation has at least one requirement, check requirements.
                {
                    bool flagsTrue = true;
                    bool isHighPriority = false;
                    foreach (string flag in convo.flags) // Check all flags
                    {
                        if (!FlagManager.Instance.GetFlag(flag)) // Check if flag is true
                        {
                            flagsTrue = false;
                        }
                        else // If flag is true
                        {
                            if (!(flag == "Act1" || flag == "Act2" || flag == "Act3" || flag == "highRep" || flag == "lowRep")) // Check if the flag is an act or rep requirement
                            {
                                isHighPriority = true; // Mark this convo as higher priority if unique
                            }
                        }
                    }
                    if (flagsTrue) // If all flags are true
                    {
                        if (isHighPriority)
                        {
                            highPriorityConvos.Add(convo);
                        }
                        else
                        { 
                            validConvos.Add(convo);
                        }
                    }
                }
                else
                {
                    validConvos.Add(convo);
                }
            }

        });

        Debug.Log($"[NPCData] Num of high priority convos: {highPriorityConvos.Count}");

        // Randomly choose from high priority conversations
        if (highPriorityConvos.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, highPriorityConvos.Count);
            Debug.Log($"[NPCData] Random Index for high priority: {randomIndex}");
            convoToWrite = highPriorityConvos[randomIndex];
        }
        
        // If there are no high priority conversations, choose from all valid conversations
        if (convoToWrite == null)
        {
            Debug.Log($"[NPCData] Num of valid conversations: {validConvos.Count}");
            
            if (validConvos.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, validConvos.Count);
                Debug.Log($"[NPCData] Random Index for valid convos: {randomIndex}");
                convoToWrite = validConvos[randomIndex];
            }
            else
            {
                Debug.LogWarning($"[NPCData] NO VIABLE CONVERSATIONS FOR {displayInfo.npcName}"); ///////////   IF NO CONVERSATIONS ARE VIABLE, THIS WILL CAUSE PROBLEMS
            }
        }

        return convoToWrite;
    }


}
