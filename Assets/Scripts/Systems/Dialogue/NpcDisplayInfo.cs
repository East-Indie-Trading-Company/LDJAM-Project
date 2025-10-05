using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Npc Info")]
public class NpcDisplayInfo : ScriptableObject
{
    public string npcName;
    public Sprite npcIcon;
    public string townName;
    public Sprite townIcon;
}
