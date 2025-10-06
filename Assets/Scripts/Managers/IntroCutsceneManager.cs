using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroCutsceneManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
