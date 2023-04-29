using _Code.Managers;
using _Code.UI;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        // Ink
        [SerializeField] private InkData inkData;
        private Story _currentStory;
        private string _currentLine;

        // UI
        [SerializeField] private DialogueUI dialogueUI;

        // Input
        private InputAction _continueAction;

       // Managers
        private GameManager _gameManager;
        
        // Tags
        private const string CharacterNameTag = "Name";
        private const string CharacterImageTag = "Image";
        private const string BackgroundTag = "BG";
        private const string NarrationTag = "Narration";
        private const string EndNarrationTag = "EndNarration";

        private void Awake()
        {
            SetUpInputSystem();
        }

        private void SetUpInputSystem()
        {
            // Set up the Input System
            _continueAction = new InputAction("Continue", InputActionType.Button);
            _continueAction.AddBinding("<Keyboard>/enter");
            _continueAction.AddBinding("<Mouse>/leftButton");
            _continueAction.AddBinding("<Gamepad>/buttonSouth");
            _continueAction.performed += ctx => ContinueWhenNotTyping();
            _continueAction.Enable();
        }

        private void Start()
        {
            _currentStory = inkData.GetStory();
        }
        private void StartStory()
        {
            _currentStory = inkData.GetStory();
            dialogueUI.Show();
            ShowNextLine();
            _continueAction.Enable();
        }

        public void StartStory(InkData data) 
        {
            inkData = data;
            _currentStory = data.GetStory();
            dialogueUI.Show();
            ShowNextLine();
            _continueAction.Enable();
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
            if (_currentStory.canContinue)
            {
                _currentLine = _currentStory.Continue();
                dialogueUI.RefreshLine(_currentLine, () => { });
                ProcessTags();
            }
            else
            {
                // End of the story
                ShowCharacterInfo();
                _currentStory.ResetState();
                dialogueUI.Hide();
                _continueAction.Disable();
            }
        }

        private void ProcessTags()
        {
            var tags = _currentStory.currentTags;
            foreach (var t in tags)
            {
                var splitTag = t.Split(':');
                var tagKey = splitTag[0].Trim();
                var tagValue = splitTag.Length > 1 ? splitTag[1].Trim() : null;

                ApplyTag(tagKey, tagValue);
            }
        }

        private void ApplyTag(string tagKey, string tagValue)
        {
            switch (tagKey)
            {
                case BackgroundTag:
                    if (int.TryParse(tagKey, out var index))
                    {
                        dialogueUI.SetBackgroundImage(inkData.backgroundImages[index]);
                    }
                    break;
                case CharacterNameTag:
                    dialogueUI.SetCharacterName(tagValue == "주인공" ? _gameManager.saveData.playerName : tagValue);
                    break;
                case CharacterImageTag:
                    if(int.TryParse(tagKey, out var imageIndex))
                    {
                        dialogueUI.SetCharacterImage(inkData.characterImages[imageIndex]);
                    }
                    break;
                case NarrationTag:
                    HideCharacterInfo();
                    break;
                case EndNarrationTag:
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
            _continueAction.Disable();
        }
    }
}
