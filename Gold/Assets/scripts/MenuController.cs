using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        //print("current scene name:" + currentScene);
    }

    public void LoadLevel(string levelName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1;
        print(currentScene);
        SceneManager.LoadScene(currentScene);
    }

    public void ShowWindow(GameObject window)
    {
        window.SetActive(true);
        
    }

    public void Pause(GameObject window)
    {
        Time.timeScale = 0;
        window.SetActive(true);
    }

    public void StopPause(GameObject window)
    {
        Time.timeScale = 1;
        window.SetActive(false);
    }

    public void HideWindow(GameObject window)
    {
        window.SetActive(false);
    }

    public void ShowInstructions()
    {
        // Assuming you have a GameObject for the instructions panel
        GameObject instructionsPanel = GameObject.FindWithTag("Instructions");
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        GameObject instructionsPanel = GameObject.FindWithTag("Instructions");
        instructionsPanel.SetActive(false);
    }
}

