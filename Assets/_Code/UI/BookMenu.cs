using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class BookMenu : MonoBehaviour
    {
        public GameObject characterMenu;
        public RectTransform spawnPoint;
        public Toggle toggle;

        private GameObject _currentInstance;

        private void Start()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                SpawnCharacterMenu();
            }
            else
            {
                DeletePrefab();
            }
        }

        private void SpawnCharacterMenu()
        {
            if(characterMenu != null && spawnPoint != null)
            {
                _currentInstance = Instantiate(characterMenu, spawnPoint.position, spawnPoint.rotation, spawnPoint);
                RectTransform rectTransform = _currentInstance.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = Vector3.one;
            }
        }

        private void DeletePrefab()
        {
            if (_currentInstance != null)
            {
                Destroy(_currentInstance);
            }
        }
    }
}
