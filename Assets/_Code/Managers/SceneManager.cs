using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Singleton
    public static SceneManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(int sceneIndex, LoadSceneMode mode)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex, mode);
    }

    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneIndex));
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadSceneAsyncCoroutine(int sceneIndex)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void LoadSceneAsync(string sceneName, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, mode));
    }

    public void LoadSceneAsync(int sceneIndex, LoadSceneMode mode)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneIndex, mode));
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadSceneAsyncCoroutine(int sceneIndex, LoadSceneMode mode)
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, mode);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
