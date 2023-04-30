using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class TitleButtons : MonoBehaviour
    {
        private GameManager _gameManager;
        [SerializeField] private Button loadButton;

        private void Start()
        {
            _gameManager = GameManager.instance;
            loadButton.gameObject.SetActive(_gameManager.IsSaveFileExist());
        }
    }
}