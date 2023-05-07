using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Dialogue.UI
{
    public class DialogueUI : MonoBehaviour
    {
        // Container
        public GameObject nameBox;
        
        // Text
        public TMP_Text nameText;
        public TMP_Text dialogueText;
        
        // Image
        public Image backgroundSprite;
        public Image characterSprite;

        // Methods
        // ReSharper disable once ParameterHidesMember
        public void SetName(string name)
        {
            nameText.text = name;
        }
        
        public void SetDialogue(string dialogue)
        {
            dialogueText.text = dialogue;
        }

        public void SetBackground(Sprite background)
        {
            backgroundSprite.sprite = background;
        }
        
        public void SetCharacter(Sprite character)
        {
            characterSprite.color = Color.white;
            characterSprite.sprite = character;
        }
        
        public void HideCharacter()
        {
            characterSprite.sprite = null;
            characterSprite.color = Color.clear;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void ShowNameBox()
        {
            nameBox.SetActive(true);
        }
        
        public void HideNameBox()
        {
            nameBox.SetActive(false);
        }
    }
}
