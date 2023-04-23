using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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