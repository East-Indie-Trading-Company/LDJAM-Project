using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroCutsceneManager : MonoBehaviour
{
    //[SerializeField] private GameObject permaUICanvas;
    public void NewGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
