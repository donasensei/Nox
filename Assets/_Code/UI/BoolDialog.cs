using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoolDialog : MonoBehaviour
{
    public Text InfoText;
    [SerializeField] private List<CanvasGroup> Groups;

    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    // Events
    public event Action OnConfirm;
    public event Action OnCancel;

    private void Awake()
    {
        gameObject.SetActive(false);
        confirmButton.onClick.AddListener(HandleConfirm);
        cancelButton.onClick.AddListener(HandleCancel);
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

    public void SetInfoText(string text)
    {
        InfoText.text = text;
    }

    private void HandleConfirm()
    {
        OnConfirm?.Invoke();
        Hide();
    }

    private void HandleCancel()
    {
        OnCancel?.Invoke();
        Hide();
    }

    private void OnDestroy()
    {
        confirmButton.onClick.RemoveListener(HandleConfirm);
        cancelButton.onClick.RemoveListener(HandleCancel);
    }
}
