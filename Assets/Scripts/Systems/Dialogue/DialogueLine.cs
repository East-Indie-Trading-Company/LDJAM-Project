using UnityEngine;

[System.Serializable]
public class DialogueLine 
{
    public string dialogueText;  // The text for the NPC's dialogue
    [SerializeField] DialogueChoice choice;
}
