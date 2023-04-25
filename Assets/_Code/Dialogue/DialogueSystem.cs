using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using System.Net.NetworkInformation;
using UnityEditor.Rendering;


public class DialogueSystem : MonoBehaviour
{
    // Ink
    [SerializeField] private List<InkData> inkDatas;
    private Story currentStory;
    private string currentLine;
    private int currentInkIdx = 0;

    // UI
    [SerializeField] private DialogueUI dialogueUI;

    // Input
    private InputAction continueAction;

    private void Awake()
    {
        SetUpInputSystem();
    }

    private void SetUpInputSystem()
    {
        // Set up the Input System
        continueAction = new InputAction("Continue", InputActionType.Button);
        continueAction.AddBinding("<Keyboard>/enter");
        continueAction.AddBinding("<Mouse>/leftButton");
        continueAction.AddBinding("<Gamepad>/buttonSouth");
        continueAction.performed += ctx => ContinueWhenNotTyping();
        continueAction.Enable();
    }

    private void Start()
    {
        // currentStory = inkDatas[0].GetStory();
        // ShowNextLine();
        dialogueUI.Hide();
        continueAction.Disable();
    }

    public void StartStory(int idx)
    {
        currentInkIdx = idx;
        currentStory = inkDatas[idx].GetStory();
        dialogueUI.Show();
        ShowNextLine();
        continueAction.Enable();
    }

    private void ContinueWhenNotTyping()
    {
        if (dialogueUI.IsTyping())
        {
            dialogueUI.FinishTyping();
        }
        else
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        if (currentStory.canContinue)
        {
            currentLine = currentStory.Continue();
            dialogueUI.RefreshLine(currentLine, () => { });
            ProcessTags();
        }
        else
        {
            // End of the story
            ShowCharacterInfo();
            dialogueUI.Hide();
            continueAction.Disable();
        }
    }

    private void ProcessTags()
    {
        List<string> tags = currentStory.currentTags;
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag.Length > 1 ? splitTag[1].Trim() : null;

            ApplyTag(tagKey, tagValue);
        }
    }

    private void ApplyTag(string tagKey, string tagValue)
    {
        switch (tagKey)
        {
            case "Name":
                dialogueUI.SetCharacterName(tagValue);
                ShowCharacterInfo();
                break;
            case "Image":
                int imageIndex = int.Parse(tagValue);
                dialogueUI.SetCharacterImage(inkDatas[currentInkIdx].characterImages[imageIndex]);
                ShowCharacterInfo();
                break;
            case "Narration":
                HideCharacterInfo();
                break;
            case "EndNarration":
                ShowCharacterInfo();
                break;
            default:
                break;
        }
    }

    private void ShowCharacterInfo()
    {
        dialogueUI.ShowCharacterImage();
        dialogueUI.ShowCharacterName();
    }

    private void HideCharacterInfo()
    {
        dialogueUI.HideCharacterImage();
        dialogueUI.HideCharacterName();
    }

    private void OnDestroy()
    {
        continueAction.Disable();
    }
}
