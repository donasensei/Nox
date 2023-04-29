using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class TypingEffect : MonoBehaviour
    {
        [SerializeField] private float typingSpeed = 0.03f;

        private bool isTyping = false;
        private Coroutine typingCoroutine = null;
        private string fullText;

        public IEnumerator TypeEffect(string line, Text textComponent, Action onComplete)
        {
            isTyping = true;
            fullText = line;
            for (int i = 0; i <= line.Length; i++)
            {
                textComponent.text = line.Substring(0, i);
                yield return new WaitForSeconds(typingSpeed);
            }
            isTyping = false;
            onComplete?.Invoke();
        }

        public void StartTyping(string line, Text textComponent, Action onComplete)
        {
            if (typingCoroutine != null)
            {
                StopTyping();
            }
            typingCoroutine = StartCoroutine(TypeEffect(line, textComponent, onComplete));
        }

        public void StopTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        public void FinishTyping(Text textComponent)
        {
            StopTyping();
            textComponent.text = fullText;
            isTyping = false;
        }

        public bool GetIsTyping()
        {
            return isTyping;
        }
    }
}
