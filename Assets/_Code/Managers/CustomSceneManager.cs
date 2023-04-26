using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private float minimumLoadingTime = 0.5f;
    [SerializeField] private LoadingScreen loadingScreenPrefab;
    private LoadingScreen loadingScreenInstance;

    // Singleton
    public static CustomSceneManager Instance { get; private set; }

    // Stack
    private Stack<int> sceneHistory;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sceneHistory = new Stack<int>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ShowLoadingScreen(bool show)
    {
        if (show)
        {
            if (loadingScreenInstance == null)
            {
                loadingScreenInstance = Instantiate(loadingScreenPrefab);
                DontDestroyOnLoad(loadingScreenInstance.gameObject);
            }
        }
        else
        {
            if (loadingScreenInstance != null)
            {
                Destroy(loadingScreenInstance.gameObject);
                loadingScreenInstance = null;
            }
        }
    }

    // Load scene by index
    public void LoadScene(int sceneIndex)
    {
        // Store the current scene index in the stack
        sceneHistory.Push(SceneManager.GetActiveScene().buildIndex);

        // Load the requested scene
        SceneManager.LoadScene(sceneIndex);
    }

    // Load scene by name
    public void LoadScene(string sceneName)
    {
        // Store the current scene index in the stack
        sceneHistory.Push(SceneManager.GetActiveScene().buildIndex);

        // Load the requested scene
        SceneManager.LoadScene(sceneName);
    }

    // Load the previous scene
    public void LoadPreviousScene()
    {
        if (sceneHistory.Count > 0)
        {
            // Pop the last scene index from the stack and load the scene
            int previousSceneIndex = sceneHistory.Pop();
            SceneManager.LoadScene(previousSceneIndex);
        }
    }
}
