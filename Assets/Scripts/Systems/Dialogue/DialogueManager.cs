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
    [SerializeField] TextMeshProUGUI townNameText;
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] TextMeshProUGUI choiceText;
    [SerializeField] Button buttonA;
    [SerializeField] Button buttonB;
    [SerializeField] CanvasGroup choicePanel;
    [SerializeField] GameObject dialogueCanvas;

    int currentLineIndex;
    DialogueConversation currentConversation;
    DialogueLine currentLine;
    string currentTextToDisplay;
    bool isTyping = false;
    bool waitingForChoice = false;

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
    void Start()
    {
        buttonA.onClick.AddListener(ChoiceA);
        buttonB.onClick.AddListener(ChoiceB);
    }


    public void StartDialogue(DialogueConversation conversation)
    {
        // Set the UI
        dialogueCanvas.SetActive(true);
        DisableChoicePanel();

        // TODO:: Assign npc info
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
        currentLine = currentConversation.lines[currentLineIndex];
        currentTextToDisplay = currentLine.dialogueText;
        StartCoroutine(StartTyping(currentTextToDisplay));

        // If choice is not null, show choices
        if (currentLine.choice != null)
        {
            waitingForChoice = true;
            // Show choices
            choicePanel.alpha = 1f;
            choicePanel.interactable = true;
            choicePanel.blocksRaycasts = true;
            choiceText.text = currentLine.choice.displayLine;
            buttonA.GetComponentInChildren<TextMeshProUGUI>().text = currentLine.choice.optionA.buttonText;
            buttonB.GetComponentInChildren<TextMeshProUGUI>().text = currentLine.choice.optionB.buttonText;
        }

    }
    IEnumerator StartTyping(string currentText)
    {
        Debug.Log($"[DialogueManager] Begin typing: {currentText}");
        isTyping = true; // Set typing flag to true
        npcText.text = "";  // Clear existing text
        allowAutoContinue = true; // Reenable autoplay
        foreach (char letter in currentText.ToCharArray())
        {
            npcText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // Wait for typing speed
        }
        isTyping = false;

        
        if (allowAutoContinue && !waitingForChoice)
        {
            yield return new WaitForSeconds(autoContinueSpeed);
            NextLine();
        }
    }

    // Skips typing or advances to the next line, should be triggered on player click
    public void AdvanceDialogue()
    {
        if (currentConversation == null || waitingForChoice) return;

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

    void ChoiceA()
    {
        Debug.Log($"You picked choice A. Response will be: {currentLine.choice.optionA.responseText}");
        waitingForChoice = false;
        DisableChoicePanel();
        // TODO:: Update inventory/rep/flags
        StartCoroutine(StartTyping(currentLine.choice.optionA.responseText));
    }
    void ChoiceB()
    {
        Debug.Log($"You picked choice B. Response will be: {currentLine.choice.optionB.responseText}");
        waitingForChoice = false;
        DisableChoicePanel();
        // TODO:: Update inventory/rep/flags
        StartCoroutine(StartTyping(currentLine.choice.optionB.responseText));
    }

    void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
        currentConversation = null;
    }

    void DisableChoicePanel()
    {
        choicePanel.alpha = 0f;
        choicePanel.interactable = false; 
        choicePanel.blocksRaycasts = false;
    }
    
}
