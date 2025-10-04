using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Object/Npc Info")]
public class NpcDisplayInfo : ScriptableObject
{
    string npcName;
    Sprite npcIcon;
    string townName;
    Sprite townIcon;
}
