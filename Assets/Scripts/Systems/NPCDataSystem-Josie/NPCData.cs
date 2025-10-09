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
    List<DialogueConversation> npcRepeatFilter = new List<DialogueConversation>();
    [SerializeField] DialogueConversation[] npcRumorDialogueLines;
    List<DialogueConversation> rumorRepeatFilter = new List<DialogueConversation>();
    [SerializeField] DialogueConversation[] npcGreetingDialogueLines;
    List<DialogueConversation> greetingRepeatFilter = new List<DialogueConversation>();

    public void Talk()
    {
        Debug.Log($"Pull {displayInfo.npcName} chat pool");
        DialogueManager.Instance.StartDialogue(PullDialogue(npcDialogueLines, npcRepeatFilter));
    }

    public void Rumor()
    {
        Debug.Log($"Pull {displayInfo.npcName} rumor pool");
        DialogueManager.Instance.StartDialogue(PullDialogue(npcRumorDialogueLines, rumorRepeatFilter));
    }

    public void Greeting(TextMeshProUGUI textComponent)
    {
        Debug.Log($"Pull {displayInfo.npcName} greeting pool");
        DialogueConversation convo = PullDialogue(npcGreetingDialogueLines, greetingRepeatFilter);

        if (convo.lines.Count > 0)
        {
            textComponent.text = convo.lines[0].dialogueText;
        }
        else
        {
            Debug.Log($"[NPCData] NO LINES!");
        }
    }


    DialogueConversation PullDialogue(DialogueConversation[] conversations, List<DialogueConversation> repeatFilter)
    {
        //Debug.Log($"[NPCData] Num of convos in this group: {conversations.Length}");
        DialogueConversation convoToWrite = null;

        List<DialogueConversation> validConvos = new List<DialogueConversation>();
        List<DialogueConversation> validConvosCopy = new List<DialogueConversation>();
        List<DialogueConversation> highPriorityConvos = new List<DialogueConversation>();
        List<DialogueConversation> highPriorityConvosCopy = new List<DialogueConversation>();

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

        // Check high priority
        if (highPriorityConvos.Count > 0) // Cannot start as empty when checking against filters
        {
            highPriorityConvosCopy = highPriorityConvos;
            convoToWrite = FilterPulls(highPriorityConvos, repeatFilter);
            if (convoToWrite == null)
            {
                // Make the list a copy, and go again
                highPriorityConvos = highPriorityConvosCopy;
                convoToWrite = FilterPulls(highPriorityConvos, repeatFilter);
            }
        }

        if (convoToWrite == null)
        {
            if (validConvos.Count > 0) // Cannot start as empty when checking against filters
            {
                validConvosCopy = validConvos;
                convoToWrite = FilterPulls(validConvos, repeatFilter);
                if (convoToWrite == null)
                {
                    // Make the list a copy, and go again
                    validConvos = validConvosCopy;
                    convoToWrite = FilterPulls(validConvos, repeatFilter);
                }
            }
            else
            {
                Debug.LogWarning($"[NPCData] NO VIABLE CONVERSATIONS FOR {displayInfo.npcName}"); ///////////   IF NO CONVERSATIONS ARE VIABLE, THIS WILL CAUSE PROBLEMS
            }
        }


        if (convoToWrite != null)
        {
            repeatFilter.Add(convoToWrite); // Add convo to the repeat filter
        }
        return convoToWrite;
    }

    DialogueConversation FilterPulls(List<DialogueConversation> convoList, List<DialogueConversation> repeatFilter)
    {
        DialogueConversation returnConvo = null;
        // If convos are empty, then reset the filter
        if (convoList.Count == 0)
        {
            // Reset the filter
            repeatFilter.Clear();
            return null;
        }
        // Pull from list, then compare against filter
        DialogueConversation convoPulled = RandomPull(convoList);
        foreach (DialogueConversation repeatConvo in repeatFilter)
        {
            // If it matches anything in the filter, remove from list, then pull again
            if (convoPulled == repeatConvo)
            {
                convoList.Remove(convoPulled);
                returnConvo = FilterPulls(convoList, repeatFilter);
                break;
            }
        }

        if (returnConvo == null) // If the convo isn't in the repeat filter, return this convo
        {
            returnConvo = convoPulled;
        }

        return returnConvo;
    }
    DialogueConversation RandomPull(List<DialogueConversation> convoList)
    {
        int randomIndex = UnityEngine.Random.Range(0, convoList.Count);
        //Debug.Log($"[NPCData] Random Index pulled: {randomIndex}");
        return convoList[randomIndex];
    }

}
