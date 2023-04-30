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
        private PlayerInput _playerInput;
        private InputAction _submitAction;
        private InputAction _clickAction;

       // Managers
        private GameManager _gameManager;
        
        // Tags
        private const string CharacterNameTag = "Name";
        private const string CharacterImageTag = "Image";
        private const string BackgroundTag = "BG";
        private const string NarrationTag = "Narration";
        private const string EndNarrationTag = "EndNarration";
        
        // Boolean
        private bool _isContinueActionTriggered;
        
        // Const
        private const string MainCharacterTag = "주인공";

        private void Awake()
        {
            SetUpInputSystem();
        }

        private void SetUpInputSystem()
        {
            // Set up the Input System
            _playerInput = FindObjectOfType<PlayerInput>();
            
            // Set up the Submit action
            _submitAction = _playerInput.actions["Submit"];
            _submitAction.performed += _ => ContinueWhenNotTyping();
            _submitAction.Enable();

            // Set up the Click action
            _clickAction = _playerInput.actions["Click"];
            _clickAction.performed += _ => OnClick();
            _clickAction.Enable();
        }

        private void Start()
        {
            _gameManager = GameManager.instance;
            _currentStory = inkData.GetStory();
            StartStory();
        }
        
        private void StartStory()
        {
            _currentStory = inkData.GetStory();
            dialogueUI.Show();
            ShowNextLine();
        }

        public void StartStory(InkData data) 
        {
            inkData = data;
            _currentStory = data.GetStory();
            dialogueUI.Show();
            ShowNextLine();
        }

        private void ContinueWhenNotTyping()
        {
            if (_isContinueActionTriggered) return;
            _isContinueActionTriggered = true;
            Invoke(nameof(PerformContinue), 0.1f);
        }

        private void PerformContinue()
        {
            if (dialogueUI.IsTyping())
            {
                dialogueUI.FinishTyping();
            }
            else
            {
                ShowNextLine();
            }
            _isContinueActionTriggered = false;
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
                    if (int.TryParse(tagValue, out var index))
                    {
                        dialogueUI.SetBackgroundImage(inkData.backgroundImages[index]);
                    }
                    break;
                case CharacterNameTag:
                    dialogueUI.SetCharacterName(tagValue == MainCharacterTag ? _gameManager.saveData.playerName : tagValue);
                    break;
                case CharacterImageTag:
                    if (int.TryParse(tagValue, out var imageIndex))
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

        #region 이벤트 함수

        private void OnClick()
        {
            ContinueWhenNotTyping();
        }

        private void OnDestroy()
        {
            _submitAction.Disable();
            _clickAction.Disable();
        }

        #endregion
        
    }
}
