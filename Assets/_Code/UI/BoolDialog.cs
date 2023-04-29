using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Code.UI
{
    public class BoolDialog : MonoBehaviour
    {
        [SerializeField] private Text infoText;
        [SerializeField] private List<CanvasGroup> groups;
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
            foreach (var group in groups)
            {
                group.interactable = false;
            }
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            foreach (var group in groups)
            {
                group.interactable = true;
            }
            gameObject.SetActive(false);
        }

        public void SetInfoText(string text)
        {
            infoText.text = text;
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
}
