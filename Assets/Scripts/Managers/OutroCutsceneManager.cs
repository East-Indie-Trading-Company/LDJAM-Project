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
        //load player vars
        economyGoalHit = PlayerPrefs.GetInt("economyGoalHit") == 1 ? true : false;
        reputationGoalHit = PlayerPrefs.GetInt("reputationGoalHit") == 1 ? true : false;
        sidedWithDragon = PlayerPrefs.GetInt("sidedWithDragon") == 1 ? true : false;

        if (sidedWithDragon)
        {
            dragonPic.SetActive(true);
            kingdomPic.SetActive(false);
        }
        else
        {
            dragonPic.SetActive(false);
            kingdomPic.SetActive(true);
        }

        SetEndText();
    }

    public void SetEndText()
    {
        if (sidedWithDragon)
        {
            if(economyGoalHit)
            {
                if (reputationGoalHit)
                {
                    cutsceneText.text = dragonEconomyReputationCopy;
                }
                else
                {
                    cutsceneText.text = dragonEconomyNoReputationCopy;
                }
            }
            else
            {
                if (reputationGoalHit)
                {
                    cutsceneText.text = dragonNoEconomyReputationCopy;
                }
                else
                {
                    cutsceneText.text = dragonNoEconomyNoReputationCopy;
                }
            }
        }
        else
        {
            if (economyGoalHit)
            {
                if (reputationGoalHit)
                {
                    cutsceneText.text = noDragonEconomyReputationCopy;
                }
                else
                {
                    cutsceneText.text = noDragonEconomyNoReputationCopy;
                }
            }
            else
            {
                if (reputationGoalHit)
                {
                    cutsceneText.text = noDragonNoEconomyReputationCopy;
                }
                else
                {
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
