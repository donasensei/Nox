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

        // Set the text color to the selected color when the button is selected
        public void OnSelect(BaseEventData eventData)
        {
            text.color = selectedColor;
        }

        // Set the text color back to the normal color when the button is deselected
        public void OnDeselect(BaseEventData eventData)
        {
            text.color = normalColor;
        }
    }
}