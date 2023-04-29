using System;
using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class ExploreButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        private static GameManager _gameManager;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private static void OnButtonClicked()
        {
            Debug.Log("Explore button clicked");
        }
    }
}
