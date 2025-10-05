using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject howToPlayObject;
    [SerializeField] private GameObject creditsObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SwitchToMainMenu()
    {
        mainMenuObject.SetActive(true);
        howToPlayObject.SetActive(false);
        creditsObject.SetActive(false);
    }

    public void SwitchToHowToPlay()
    {
        mainMenuObject.SetActive(false);
        howToPlayObject.SetActive(true);
        creditsObject.SetActive(false);
    }

    public void SwitchToCedits()
    {
        mainMenuObject.SetActive(false);
        howToPlayObject.SetActive(false);
        creditsObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDiscord()
    {
        Application.OpenURL("https://discord.gg/PQMKkBhMUv");
    }
}
