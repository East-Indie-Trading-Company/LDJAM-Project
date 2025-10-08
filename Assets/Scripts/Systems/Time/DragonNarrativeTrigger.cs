using UnityEngine;
using UnityEngine.Playables;

public class DragonNarrativeTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DragonTaxCollectorManager taxCollector;

    [Header("Timeline (optional)")]
    [SerializeField] private PlayableDirector timeline;


    //[Header("Dialogue (optional)")]
    //[SerializeField] private DialogueSystem dialogueSystem; // substitui pelo teu sistema real
    //[SerializeField] private string dialogueKey = "dragon_encounter_intro";

    private void OnEnable()
    {
        if (taxCollector != null)
            taxCollector.OnDragonEncounter += HandleDragonEncounter;
    }

    private void OnDisable()
    {
        if (taxCollector != null)
            taxCollector.OnDragonEncounter -= HandleDragonEncounter;
    }

    private void HandleDragonEncounter()
    {
        Debug.Log("[DragonNarrativeTrigger] Dragon encounter triggered starting narrative.");

        if (timeline != null)
        {
            timeline.Play();
        }

        if (FlagManager.Instance.GetFlag("Act3"))
        {
            Debug.Log("[DragonNarrativeTrigger] Trigger narative Act3");
            FlagManager.Instance.SetFlag("Dragon3", true);
            
        }
        else if (FlagManager.Instance.GetFlag("Act2"))
        {
            Debug.Log("[DragonNarrativeTrigger] Trigger narative Act2");
            
            FlagManager.Instance.SetFlag("Dragon2", true);
        }
        else if (FlagManager.Instance.GetFlag("Act1"))
        {
            Debug.Log("[DragonNarrativeTrigger] Trigger narative Act1");
            
            FlagManager.Instance.SetFlag("Dragon1", true);

        }
        else
        {
            Debug.Log("[DragonNarrativeTrigger] NO ACT IS TRIGGERED");
        }

        Debug.Log("[DragonNarrativeTrigger] Dialogue finished, check for flag");

        
    }
}
