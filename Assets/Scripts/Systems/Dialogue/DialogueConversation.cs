using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Conversation")]
public class DialogueConversation : ScriptableObject
{
    [SerializeField]
    public string npcName;
    [SerializeField]
    public List<DialogueLine> lines;

    // TODO:: Add list of flags required
}
