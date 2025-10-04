using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Choice")]
public class DialogueChoice : ScriptableObject
{
    [SerializeField]
    DialogueOption optionA; // The text displayed on the choice button
    [SerializeField]
    DialogueOption optionB; // The text displayed on the choice button
    [SerializeField]
    string DisplayLine; // Branching NPC dialogue based on player choice


}

[System.Serializable]
public class DialogueOption
{
    string buttonText;
    string responseText;

    string flagToRaise;

    // TODO:: Inventory effect

    // TODO:: Reputation effect
}