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
        Debug.Log("[DragonNarrativeTrigger] Dragon encounter triggered — starting narrative.");

        if (timeline != null)
        {
            timeline.Play();
        }

        //if (dialogueSystem != null && !string.IsNullOrEmpty(dialogueKey))
        //{
        //    dialogueSystem.StartDialogue(dialogueKey);
        //}

        // Aqui podes pausar gameplay, bloquear controlos, etc.
    }
}
