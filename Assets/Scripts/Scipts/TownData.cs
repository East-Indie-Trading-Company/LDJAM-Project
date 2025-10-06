using UnityEngine;

public class TownData : MonoBehaviour
{
    public string townName;
    public NPCData npcData;

    [Header("Travel Settings")]
    public float travelMultiplier = 1f;
    // 👆 You can tweak this per town later if some are harder to reach.
}