using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoadUI : MonoBehaviour
{
    
    [SerializeField] private GameObject permaUICanvas;
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("MapLoadUI Scene Loaded: " + scene.name);
        if (scene.name == "Map")
        {
            permaUICanvas.SetActive(true);
        }
    }
}
