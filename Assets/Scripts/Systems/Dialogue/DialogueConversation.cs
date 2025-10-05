using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Conversation")]
public class DialogueConversation : ScriptableObject
{
    public NpcDisplayInfo npc;
    public List<DialogueLine> lines;
    public bool isOneOff;
    public string[] flags;
    public bool hasPlayed;

    // TODO:: Add list of flags required
}
