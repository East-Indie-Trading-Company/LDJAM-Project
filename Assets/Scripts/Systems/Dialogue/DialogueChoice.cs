using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Dialogue Choice")]
public class DialogueChoice : ScriptableObject
{
    public DialogueOption optionA;
    public DialogueOption optionB;
    
    public string displayLine; // Dialogue informing player about choice


}

[System.Serializable]
public class DialogueOption
{
    public string buttonText;
    public string responseText;

    public string flagToRaise;

    // TODO:: Inventory effect

    // TODO:: Reputation effect
}