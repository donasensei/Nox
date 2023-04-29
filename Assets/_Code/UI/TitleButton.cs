using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class TitleButton : MonoBehaviour
    {
        public Button LoadButton;

        private void Start()
        {
            if (!GameManager.checkSaveFileExist)
            {
                LoadButton.gameObject.SetActive(false);
            }
            else
            {
                LoadButton.gameObject.SetActive(true);
            }
        }
    }
}
