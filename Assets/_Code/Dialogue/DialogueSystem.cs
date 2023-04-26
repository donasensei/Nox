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
    [SerializeField] private InkData inkData;
    private Story currentStory;
    private string currentLine;
    // private int currentInkIdx = 0;

    // UI
    [SerializeField] private DialogueUI dialogueUI;

    // Input
    private InputAction continueAction;

    [SerializeField] private bool startStoryFirst = false;
    public bool StartStoryFirst { get { return startStoryFirst; } set { startStoryFirst = value; } }

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
        if(!startStoryFirst)
        {
            dialogueUI.Hide();
            continueAction.Disable();
        }
        else
        {
            StartStory();
        }
    }

    public void StartStory()
    {
        currentStory = inkData.GetStory();
        dialogueUI.Show();
        ShowNextLine();
        continueAction.Enable();
    }

    public void StartStory(InkData data) 
    {
        inkData = data;
        currentStory = data.GetStory();
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
        int parsedValue;
        switch (tagKey)
        {
            case "BG":
                if (int.TryParse(tagValue, out parsedValue))
                {
                    dialogueUI.SetBackgroundImage(inkData.backgroundImages[parsedValue]);
                }
                else
                {
                    Debug.LogError("Invalid tag value for 'BG': " + tagValue);
                }
                break;
            case "Name":
                if (tagValue == "¡÷¿Œ∞¯")
                {
                    dialogueUI.SetCharacterName(GameManager.Instance.SaveData.playerName);
                }
                else
                {
                    dialogueUI.SetCharacterName(tagValue);
                }
                ShowCharacterInfo();
                break;
            case "Image":
                if (int.TryParse(tagValue, out parsedValue))
                {
                    dialogueUI.SetCharacterImage(inkData.characterImages[parsedValue]);
                }
                else
                {
                    Debug.LogError("Invalid tag value for 'Image': " + tagValue);
                }
                ShowCharacterInfo();
                break;
            case "Narration":
                HideCharacterInfo();
                break;
            case "EndNarration":
                ShowCharacterInfo();
                break;
            case "Scene":
                if(GameManager.Instance.state == GameManager.GameState.Explore)
                {
                    GameManager.Instance.SaveData.currentDay += 1;
                    GameManager.Instance.state = GameManager.GameState.MainMenu;
                }
                CustomSceneManager.Instance.LoadScene(tagValue);
                break;
            case "Stone":
                Debug.Log("Tag GET");
                if (int.TryParse(tagValue, out parsedValue))
                {
                    Debug.Log(parsedValue);
                    GameManager.Instance.SaveData.stones += (uint)(parsedValue);
                }
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
