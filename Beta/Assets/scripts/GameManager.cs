using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Variables for analytics
    [HideInInspector] public int numberOfDeath;
    [HideInInspector] public int HPLost;
    [HideInInspector] public float timer;
    [HideInInspector] public string currentLevelName;
    [HideInInspector] public int numberOfKills;
    

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the GameManager from being destroyed on scene load
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize numberOfDeath or any other necessary setup here
        Initialize();
        
    }

    private void Initialize()
    {
        // Reset or initialize numberOfDeath only when a new level is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Initialize current level name
        currentLevelName = SceneManager.GetActiveScene().name;
        // Initialize death count
        numberOfDeath = 0;
        // Initilize HP Lost
        HPLost = 0;
        // Initialize timer
        timer = 0f;
        // Initialize kills
        numberOfKills = 0;
        
    }

    private void Update()
    {
        // Update the timer by adding the time since the last frame
        timer += Time.deltaTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset numberOfDeath when a new level is loaded, not on reloads
        if (IsNewLevel(scene))
        {
            numberOfDeath = 0;
            HPLost = 0;
            timer = 0f;
            numberOfKills = 0;
            
        }
    }

    private bool IsNewLevel(Scene scene)
    {
        // Implement your logic to determine if the loaded scene is a new level
        if (scene.name != currentLevelName)
        {
            currentLevelName = scene.name;
            return true;
        }
        return false; 
    }

    private void OnDestroy()
    {
        // Clean up the event listener when the GameManager is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevelName);
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
