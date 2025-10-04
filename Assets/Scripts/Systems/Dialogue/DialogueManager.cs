using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float autoContinueSpeed = 3f;
    [SerializeField] bool allowAutoContinue = true;


    [Header("UI References")]
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] GameObject dialogueCanvas;

    int currentLineIndex;
    DialogueConversation currentConversation;
    string currentTextToDisplay;
    bool isTyping = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartDialogue(DialogueConversation conversation)
    {
        // TODO:: Make UI visible
        if (conversation.lines[0] != null)
        {
            currentConversation = conversation;
            // Check if it is a one off and if true, set hasPlayed to true
            currentLineIndex = 0;
            ShowDialogue();

        }
        else
        {
            Debug.LogWarning("[Dialogue Manager] This conversation hase no lines");
        }
    }

    void ShowDialogue()
    {
        if (currentConversation == null) return;
        StopAllCoroutines();
        if (currentLineIndex >= currentConversation.lines.Count)
        {
            EndDialogue();
            return;
        }
        currentTextToDisplay = currentConversation.lines[currentLineIndex].dialogueText;
        StartCoroutine(StartTyping(currentTextToDisplay));
        // TODO:: if choice is not null, show choices

    }
    IEnumerator StartTyping(string currentText)
    {
        isTyping = true; // Set typing flag to true
        npcText.text = "";  // Clear existing text
        allowAutoContinue = true; // Reenable autoplay
        foreach (char letter in currentText.ToCharArray())
        {
            npcText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Wait for typing speed
        }
        isTyping = false;

        yield return new WaitForSeconds(autoContinueSpeed);
        if (allowAutoContinue)
        {
            NextLine();
        }
    }

    // Skips typing or advances to the next line, should be triggered on player click
    public void AdvanceDialogue()
    {
        if (currentConversation == null) return;

        if (isTyping)
        {
            EndTypingEarly();
        }
        else
        {
            allowAutoContinue = false;
            StopAllCoroutines();
            NextLine();
        }
    }

    // Complete full dailogue line immediately
    void EndTypingEarly()
    {
        if (!isTyping) return;
        StopAllCoroutines(); // Stop the typing coroutine
        npcText.text = currentTextToDisplay; // Display full line of text immediately
        isTyping = false;
    }
    // Procede to the next line
    void NextLine()
    {
        currentLineIndex++;
        ShowDialogue();
    }

    void Choice()
    {

    }

    void EndDialogue()
    {
        // TODO:: Close ui
        currentConversation = null;
    }
    
}
