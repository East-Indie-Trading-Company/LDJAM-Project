using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class OutroCutsceneManager : MonoBehaviour
{
    [SerializeField] private bool economyGoalHit = false;
    [SerializeField] private bool reputationGoalHit = false;
    [SerializeField] private bool sidedWithDragon = false;

    [SerializeField] private GameObject dragonPic;
    [SerializeField] private GameObject kingdomPic;

    [SerializeField] private TextMeshProUGUI cutsceneText;

    [SerializeField] private string dragonEconomyReputationCopy = "";
    [SerializeField] private string dragonEconomyNoReputationCopy = "";
    [SerializeField] private string dragonNoEconomyReputationCopy = "";
    [SerializeField] private string dragonNoEconomyNoReputationCopy = "";
    [SerializeField] private string noDragonEconomyReputationCopy = "";
    [SerializeField] private string noDragonEconomyNoReputationCopy = "";
    [SerializeField] private string noDragonNoEconomyReputationCopy = "";
    [SerializeField] private string noDragonNoEconomyNoReputationCopy = "";

    public void Start()
    {
        if (Trading.InventoryManager.Instance.Gold >= GameManager.Instance.economyGoal)
        {
            economyGoalHit = true;
        }
        if (ReputationManager.Instance.GetReputation() > 0)
        {
            reputationGoalHit = true;
        }
        if (FlagManager.Instance.GetFlag("EndGameDragon"))
        {
            Debug.Log("[OutroCutsceneManager] Set scene: End game dragon");
            dragonPic.SetActive(true);
            kingdomPic.SetActive(false);
        }
        else if (FlagManager.Instance.GetFlag("EndGameKingdom"))
        {
            Debug.Log("[OutroCutsceneManager] Set scene: End game kingdom");
            dragonPic.SetActive(false);
            kingdomPic.SetActive(true);
        }

        SetEndText();
    }

    public void SetEndText()
    {
        if (FlagManager.Instance.GetFlag("EndGameDragon"))
        {
            if(economyGoalHit)
            {
                if (reputationGoalHit)
                {
                    Debug.Log("[OutroCutsceneManager] End scene: dragonEconomyReputation");
                    cutsceneText.text = dragonEconomyReputationCopy;
                }
                else
                {
                    Debug.Log("[OutroCutsceneManager] End scene: dragonEconomyNoReputation");
                    cutsceneText.text = dragonEconomyNoReputationCopy;
                }
            }
            else
            {
                if (reputationGoalHit)
                {
                    Debug.Log("[OutroCutsceneManager] End scene: dragonNoEconomyReputation");
                    cutsceneText.text = dragonNoEconomyReputationCopy;
                }
                else
                {
                    Debug.Log("[OutroCutsceneManager] End scene: dragonNoEconomyNoReputation");
                    cutsceneText.text = dragonNoEconomyNoReputationCopy;
                }
            }
        }
        else if (FlagManager.Instance.GetFlag("EndGameKingdom"))
        {
            if (economyGoalHit)
            {
                if (reputationGoalHit)
                {
                    Debug.Log("[OutroCutsceneManager] End scene: noDragonEconomyReputation");
                    cutsceneText.text = noDragonEconomyReputationCopy;
                }
                else
                {
                    Debug.Log("[OutroCutsceneManager] End scene: noDragonEconomyNoReputation");
                    cutsceneText.text = noDragonEconomyNoReputationCopy;
                }
            }
            else
            {
                if (reputationGoalHit)
                {
                    Debug.Log("[OutroCutsceneManager] End scene: noDragonNoEconomyReputation");
                    cutsceneText.text = noDragonNoEconomyReputationCopy;
                }
                else
                {
                    Debug.Log("[OutroCutsceneManager] End scene: noDragonNoEconomyNoReputation");
                    cutsceneText.text = noDragonNoEconomyNoReputationCopy;
                }
            }
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
