using UnityEngine;
using UnityEngine.EventSystems;

namespace _Code.UI
{
    public class TextButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public GameObject activatedObject;

        public void OnSelect(BaseEventData eventData)
        {
            activatedObject.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            activatedObject.SetActive(false);
        }
    }
}