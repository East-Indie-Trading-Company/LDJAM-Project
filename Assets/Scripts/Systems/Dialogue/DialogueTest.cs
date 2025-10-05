using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [Header("Test Dialogue")]
    public DialogueConversation testConversation;

    [ContextMenu("Run Dialogue Test")]
    public void RunTests()
    {
        Debug.Log("Start test conversation");
        DialogueManager.Instance.StartDialogue(testConversation);
    }
    [ContextMenu("Pretend to Click")]
    public void PrentendClick()
    {
        Debug.Log("Click!");
        DialogueManager.Instance.AdvanceDialogue();
    }
}
