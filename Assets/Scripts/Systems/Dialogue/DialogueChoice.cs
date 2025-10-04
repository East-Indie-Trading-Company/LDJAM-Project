using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Choice")]
public class DialogueChoice : ScriptableObject
{
    [SerializeField]
    string choiceText; // The text displayed on the choice button
    [SerializeField]
    string responseLine; // Branching NPC dialogue based on player choice

    // TODO:: Inventory effect
    // TODO:: Reputation effect
    // TODO:: Flag to raise

}
