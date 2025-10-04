using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Conversation")]
public class DialogueConversation : ScriptableObject
{
    public string npcName;
    public List<DialogueLine> lines;
    public bool hasPlayed;

    // TODO:: Add list of flags required
}
