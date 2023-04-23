using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolDialog : MonoBehaviour
{
    public Text InfoText;
    public List<CanvasGroup> Groups;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        foreach (var group in Groups)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        foreach (var group in Groups)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        gameObject.SetActive(false);
    }
}
