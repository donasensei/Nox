using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameInputPanel : MonoBehaviour
{
    public CanvasGroup SaveSlots;
    public InputField playerNameInputField;
    public Button confirmButton;
    public Button cancelButton;

    public void Show()
    {
        SaveSlots.interactable = false;
        SaveSlots.blocksRaycasts = false;
        gameObject.SetActive(true);
        playerNameInputField.ActivateInputField();
    }

    public void Hide()
    {
        playerNameInputField.text = string.Empty;
        SaveSlots.interactable = true;
        SaveSlots.blocksRaycasts = true;
        gameObject.SetActive(false);
    }
}
