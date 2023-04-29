using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class DialogueUI : MonoBehaviour
    {
        // Texts
        [SerializeField] private Text characterName;
        [SerializeField] private Text dialogueText;

        // Images
        [SerializeField] private Image characterImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TypingEffect typing;

        // UI
        [SerializeField] private GameObject characterBox;

        public void SetCharacterName(string name)
        {
            characterName.text = name;
        }

        public void SetCharacterImage(Sprite sprite)
        {
            characterImage.sprite = sprite;
        }
        public void SetBackgroundImage(Sprite sprite)
        {
            backgroundImage.sprite = sprite;
        }

        public void RefreshLine(string dialogue, Action onComplete)
        {
            typing.StartTyping(dialogue, dialogueText, onComplete);
        }

        public void FinishTyping()
        {
            typing.FinishTyping(dialogueText);
        }

        public bool IsTyping()
        {
            return typing.GetIsTyping();
        }

        public void HideCharacterImage()
        {
            characterBox.gameObject.SetActive(false);
        }

        public void HideCharacterName()
        {
            characterName.gameObject.SetActive(false);
        }

        public void ShowCharacterImage()
        {
            characterBox.gameObject.SetActive(true);
        }

        public void ShowCharacterName()
        {
            characterName.gameObject.SetActive(true);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide(){
            this.gameObject.SetActive(false);
        }
    }
}
