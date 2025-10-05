using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMenu : MonoBehaviour
{
    // Call this from your Play button
    public void PlayLevel1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene("TestLocalMika");
    }
}
