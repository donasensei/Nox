using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class PrevButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {

        }
    }
}
