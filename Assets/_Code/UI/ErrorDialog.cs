using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class ErrorDialog : MonoBehaviour
    {
        private const float Delay = 1.0f;
        [SerializeField] private Text errorText;
        public List<CanvasGroup> Groups;

        public Action OnDialogHidden;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            /*
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            OnInputDetected();
        }
        */
        }

        private void OnInputDetected()
        {
            StartCoroutine(HideAfterDelay(delay: Delay));
        }

        private IEnumerator HideAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Hide();
        }

        public void Show()
        {
            foreach (var group in Groups)
            {
                group.interactable = false;
                group.blocksRaycasts = false;
            }
            gameObject.SetActive(true);
            StartCoroutine(HideAfterDelay(delay: Delay));
        }

        public void Hide()
        {
            foreach (var group in Groups)
            {
                group.interactable = true;
                group.blocksRaycasts = true;
            }
            gameObject.SetActive(false);

            OnDialogHidden?.Invoke(); // Invoke the event after hiding the dialog
        }

        public void SetErrorText(string text)
        {
            errorText.text = text;
        }
    }
}
