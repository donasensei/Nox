using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    // Texts
    [SerializeField] private Text characterName;
    [SerializeField] private Text dialogueText;

    // Images
    [SerializeField] private Image characterImage;
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
        //characterImage.color = Color.clear;
        characterBox.gameObject.SetActive(false);
    }

    public void HideCharacterName()
    {
        // characterName.text = null;
        characterName.gameObject.SetActive(false);
    }

    public void ShowCharacterImage()
    {
        //characterImage.color = Color.white;
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
