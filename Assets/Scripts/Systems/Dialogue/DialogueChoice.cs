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

    [Header("This is a float from -1 to 1")]
    public float reputationChangeValue = 0;
    [Header("Pick whether this number adds or subtracts")]
    public Effect reputationEffect;

    [Header("Item to change")]
    public Trading.ItemSO item;
    public int itemChangeValue = 0;
    [Header("Pick whether this number adds or subtracts")]
    public Effect itemEffect;

    [Header("Gold integer")]
    public int goldChangeValue = 0;
    [Header("Pick whether this number adds or subtracts")]
    public Effect goldEffect;

}