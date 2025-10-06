using UnityEngine;
using UnityEngine.Playables;

public class DragonNarrativeTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DragonTaxCollectorManager taxCollector;

    [Header("Timeline (optional)")]
    [SerializeField] private PlayableDirector timeline;

    [Header("Dragon Conversations")]
    [SerializeField] private DialogueConversation act1Convo;
    [SerializeField] private DialogueConversation act2Convo;
    [SerializeField] private DialogueConversation act3Convo;

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
            DialogueManager.Instance.StartDialogue(act3Convo);
        }
        else if (FlagManager.Instance.GetFlag("Act2"))
        {
            Debug.Log("[DragonNarrativeTrigger] Trigger narative Act2");
            DialogueManager.Instance.StartDialogue(act2Convo);
        }
        else if (FlagManager.Instance.GetFlag("Act1"))
        {
            Debug.Log("[DragonNarrativeTrigger] Trigger narative Act1");
            DialogueManager.Instance.StartDialogue(act1Convo);
        }
        else
        {
            Debug.Log("[DragonNarrativeTrigger] NO ACT IS TRIGGERED");
        }
        //if (dialogueSystem != null && !string.IsNullOrEmpty(dialogueKey))
        //{
        //    dialogueSystem.StartDialogue(dialogueKey);
        //}

        // Aqui podes pausar gameplay, bloquear controlos, etc.
    }
}
