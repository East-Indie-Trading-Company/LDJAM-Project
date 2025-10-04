using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField]
    string dialogueText;  // The text for the NPC's dialogue
    [SerializeField] DialogueChoice choiceOne;
    [SerializeField] DialogueChoice choiceTwo;
}
