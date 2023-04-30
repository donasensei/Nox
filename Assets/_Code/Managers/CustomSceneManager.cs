using System.Collections.Generic;
using _Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.Managers
{
    public class CustomSceneManager : MonoBehaviour
    {
        // Singleton
        public static CustomSceneManager instance { get; private set; }

        // Stack
        private Stack<int> _sceneHistory;

        private void Awake()
        {
            // Singleton setup
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                _sceneHistory = new Stack<int>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Load scene by index
        public void LoadScene(int sceneIndex)
        {
            // Store the current scene index in the stack
            _sceneHistory.Push(SceneManager.GetActiveScene().buildIndex);

            // Load the requested scene
            SceneManager.LoadScene(sceneIndex);
        }

        // Load scene by name
        public void LoadScene(string sceneName)
        {
            // Store the current scene index in the stack
            _sceneHistory.Push(SceneManager.GetActiveScene().buildIndex);

            // Load the requested scene
            SceneManager.LoadScene(sceneName);
        }

        // Load the previous scene
        public void LoadPreviousScene()
        {
            if (_sceneHistory.Count <= 0) return;
            // Pop the last scene index from the stack and load the scene
            var previousSceneIndex = _sceneHistory.Pop();
            SceneManager.LoadScene(previousSceneIndex);
        }
    }
}
