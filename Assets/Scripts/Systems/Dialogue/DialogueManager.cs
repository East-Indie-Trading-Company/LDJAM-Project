using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float autoContinueSpeed = 3f;
    [SerializeField] bool allowAutoContinue = true;


    [Header("UI References")]
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] Image npcPortrait;
    [SerializeField] TextMeshProUGUI townNameText;
    [SerializeField] Image townIcon;
    [SerializeField] TextMeshProUGUI npcText;
    [SerializeField] TextMeshProUGUI choiceText;
    [SerializeField] Button buttonA;
    [SerializeField] Button buttonB;
    [SerializeField] CanvasGroup choicePanel;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] GameObject marketCanvas;

    int currentLineIndex;
    DialogueConversation currentConversation;
    DialogueLine currentLine;
    string currentTextToDisplay;
    bool isTyping = false;
    bool waitingForChoice = false;
    bool isDragon = false;
    string raiseFlag;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        dialogueCanvas.SetActive(false);
    }
    void Start()
    {
        buttonA.onClick.AddListener(ChoiceA);
        buttonB.onClick.AddListener(ChoiceB);
    }

    // This function is here in leu of a player input system
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {
            AdvanceDialogue();
        }
    }


    public void StartDialogue(DialogueConversation conversation)
    {
        // Set the UI
        dialogueCanvas.SetActive(true);
        DisableChoicePanel();
        if (conversation.npc != null)
        {
            if (conversation.npc.townName != "The Dragon is Here to Collect")
            {
                isDragon = false;
                
            }
            else
            {
                Debug.Log("[Dialogue Manager] Talking to dragon!");
                isDragon = true;
            }
            townNameText.text = conversation.npc.townName;
            townIcon.sprite = conversation.npc.townIcon;
            npcNameText.text = conversation.npc.npcName;
            npcPortrait.sprite = conversation.npc.npcIcon;
            
        }
        else
        {
            Debug.LogWarning("[Dialogue Manager] NPC info not assigned!");
        }
        

        if (conversation.lines.Count > 0)
        {
            currentConversation = conversation;
            // Check if it is a one off and if true, set hasPlayed to true
            if (conversation.isOneOff)
            {
                conversation.hasPlayed = true;
            }
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
            // Set text
            choiceText.text = currentLine.choice.displayLine;
            buttonA.GetComponentInChildren<TextMeshProUGUI>().text = currentLine.choice.optionA.buttonText;
            buttonB.GetComponentInChildren<TextMeshProUGUI>().text = currentLine.choice.optionB.buttonText;
        }

    }
    IEnumerator StartTyping(string currentText)
    {
        //Debug.Log($"[DialogueManager] Begin typing: {currentText}");
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
        bool itemChangeValid = true;
        bool goldChangeValid = true;
        if (isTyping) return;
        Debug.Log($"You picked choice A. Response will be: {currentLine.choice.optionA.responseText}");
        
        
        // Inventory choice 
        switch (currentLine.choice.optionA.itemEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                Trading.InventoryManager.Instance.AddItem(currentLine.choice.optionA.item, currentLine.choice.optionA.itemChangeValue);
                break;
            case Effect.REMOVE:
                itemChangeValid = Trading.InventoryManager.Instance.RemoveItem(currentLine.choice.optionA.item, currentLine.choice.optionA.itemChangeValue);
                break;
        }

        // Gold choice 
        switch (currentLine.choice.optionA.goldEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                Trading.InventoryManager.Instance.AddGold(currentLine.choice.optionA.goldChangeValue);
                break;
            case Effect.REMOVE:
                goldChangeValid = Trading.InventoryManager.Instance.RemoveGold(currentLine.choice.optionA.goldChangeValue);
                break;
        }

        // Rep choice 
        switch (currentLine.choice.optionB.reputationEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                ReputationManager.Instance.AddReputation(currentLine.choice.optionA.reputationChangeValue);
                break;
            case Effect.REMOVE:
                ReputationManager.Instance.SubtractReputation(currentLine.choice.optionA.reputationChangeValue);
                break;
        }

        Debug.Log($"[DialogueManager] Item change: {itemChangeValid}, Gold change: {goldChangeValid}");
        if (itemChangeValid && goldChangeValid)
        {
            DisableChoicePanel();
            waitingForChoice = false;

            // Raise flag
            raiseFlag = currentLine.choice.optionA.flagToRaise;
            if (raiseFlag != null)
            {
                FlagManager.Instance.SetFlag(raiseFlag, true);
            }
            currentTextToDisplay = currentLine.choice.optionA.responseText;
            StartCoroutine(StartTyping(currentLine.choice.optionA.responseText));
        }
        else
        {
            choiceText.text = "You don't have enough to choose that option";
        }
    }
    void ChoiceB()
    {
        bool itemChangeValid = true;
        bool goldChangeValid = true;
        if (isTyping) return;
        Debug.Log($"You picked choice B. Response will be: {currentLine.choice.optionB.responseText}");

        // Inventory choice 
        switch (currentLine.choice.optionB.itemEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                Trading.InventoryManager.Instance.AddItem(currentLine.choice.optionB.item, currentLine.choice.optionB.itemChangeValue);
                break;
            case Effect.REMOVE:
                itemChangeValid = Trading.InventoryManager.Instance.RemoveItem(currentLine.choice.optionB.item, currentLine.choice.optionB.itemChangeValue);
                break;
        }

        // Gold choice 
        switch (currentLine.choice.optionB.goldEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                Trading.InventoryManager.Instance.AddGold(currentLine.choice.optionB.goldChangeValue);
                break;
            case Effect.REMOVE:
                goldChangeValid = Trading.InventoryManager.Instance.RemoveGold(currentLine.choice.optionB.goldChangeValue);
                break;
        }
        // Rep choice 
        switch (currentLine.choice.optionB.reputationEffect)
        {
            case Effect.NONE:
                break;
            case Effect.ADD:
                ReputationManager.Instance.AddReputation(currentLine.choice.optionB.reputationChangeValue);
                break;
            case Effect.REMOVE:
                ReputationManager.Instance.SubtractReputation(currentLine.choice.optionB.reputationChangeValue);
                break;
        }

        Debug.Log($"[DialogueManager] Item change: {itemChangeValid}, Gold change: {goldChangeValid}");
        if (itemChangeValid && goldChangeValid)
        {
            DisableChoicePanel();
            waitingForChoice = false;

            // Raise flag
            raiseFlag = currentLine.choice.optionB.flagToRaise;
            if (raiseFlag != null)
            {
                Debug.Log($"[DialogueManager] Raise flag: {raiseFlag}");
                FlagManager.Instance.SetFlag(raiseFlag, true);
            }
            currentTextToDisplay = currentLine.choice.optionB.responseText;
            StartCoroutine(StartTyping(currentLine.choice.optionB.responseText));
        }
        else
        {
            choiceText.text = "You don't have enough to choose that option";
        }
    }

    void EndDialogue()
    {
        if (!isDragon)
        {
            marketCanvas.SetActive(true);
        }
        else
        {
            // Check for game over
            if (FlagManager.Instance.GetFlag("DragonDeath"))
            {
                SceneManager.LoadScene("PlayerDeath");
            }

            // Check for end game dragon
            if (FlagManager.Instance.GetFlag("EndGameDragon"))
            {
                PlayerPrefs.GetInt("sidedWithDragon", 1);
                if (Trading.InventoryManager.Instance.Gold >= 20000)
                {
                    PlayerPrefs.SetInt("economyGoalHit", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("economyGoalHit", 0);
                }

                if (ReputationManager.Instance.GetReputation() > 0)
                {
                    PlayerPrefs.SetInt("reputationGoalHit", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("reputationGoalHit", 0);
                }
                //Call ending
                SceneManager.LoadScene("EndingCutscene");
            }


            // Check for end game kingdom
            if (FlagManager.Instance.GetFlag("EndGameKingdom"))
            {
                PlayerPrefs.SetInt("sidedWithDragon", 0);
                if (Trading.InventoryManager.Instance.Gold >= 20000)
                {
                    PlayerPrefs.SetInt("economyGoalHit", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("economyGoalHit", 0);
                }

                if (ReputationManager.Instance.GetReputation() > 0)
                {
                    PlayerPrefs.SetInt("reputationGoalHit", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("reputationGoalHit", 0);
                }
                //Call ending
                SceneManager.LoadScene("EndingCutscene");
            }
        }
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
