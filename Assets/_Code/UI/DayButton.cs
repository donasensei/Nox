using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Code.UI
{
    public class DayButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public Text text;

        // Text Colors
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;

        private void Start()
        {
            text.color = normalColor;
        }

        public void OnSelect(BaseEventData eventData)
        {
            text.color = selectedColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            text.color = normalColor;
        }
    }
}