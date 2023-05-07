using System;
using System.Collections;
using _Code.Dialogue.UI;
using _Code.Managers;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using EasyTransition;
using UnityEngine.Serialization;

namespace _Code.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        // Ink Data
        [SerializeField] private InkData inkData;
        private Story _currentStory;
        private string _currentLine;
        
        // UI
        [SerializeField] private DialogueUI dialogueUI;
    
        // Coroutine Variables
        [SerializeField] private float textSpeed = 0.05f;
        private Coroutine _typingCoroutine;
        private bool _isTyping;
        private bool _isContinueActionTriggered;

        // Managers
        private GameManager _gameManager;
        [SerializeField] private TransitionManager transitionManager;
        
        // Tags
        private const string CharacterNameTag = "Name";
        private const string CharacterImageTag = "Image";
        private const string BackgroundTag = "BG";
        private const string NarrationTag = "Narration";
        private const string EndNarrationTag = "EndNarration";
        private const string SceneChangeTag = "SceneChange";
        private const string MainCharacterTag = "MainCharacter";
        
        // Input
        private PlayerInput _playerInput;
        private InputAction _submitAction;
        private InputAction _clickAction;
        
        // Bool
        private bool _isSceneChanging;
        
        #region Input Methods
        
        private void SetUpInputSystem()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            
            _playerInput = FindObjectOfType<PlayerInput>();
            
            _submitAction = _playerInput.actions["Submit"];
            _submitAction.performed += _ => ContinueWhenNotTyping();
            _submitAction.Enable();

            _clickAction = _playerInput.actions["Click"];
            _clickAction.performed += _ => OnClick();
            _clickAction.Enable();
        }

        private void OnClick()
        {
            ContinueWhenNotTyping();
        }
        
        private void ContinueWhenNotTyping()
        {
            if (_isContinueActionTriggered) return;
            _isContinueActionTriggered = true;
            Invoke(nameof(PerformContinue), 0.1f);
        }
        
        private void PerformContinue()
        {
            if(_isSceneChanging) return;
            if (_isTyping)
            { 
                FinishWriting();
            }
            else
            {
                ShowNextLine();
            }
            _isContinueActionTriggered = false;
        }

        #endregion
        private void Awake()
        {
            SetUpInputSystem();
        }

        private void Start()
        {
            StartStory();
            _gameManager = GameManager.instance;
        }
        
        public void SetInkData(InkData data)
        {
            inkData = data;
            StartStory();
        }
        
        private void StartStory()
        {
            _currentStory = inkData.GetStory();
            ShowNextLine();
        }
        
        private void ShowNextLine()
        {
            if (_currentStory.canContinue)
            {
                _currentLine = _currentStory.Continue();
                ProcessTags();
                StartWriting(_currentLine, () => { });
            }
            else
            {
                dialogueUI.Hide();
            }
        }

        #region 텍스트 코루틴

        private void StartWriting(string text, Action onComplete)
        {
            if (_typingCoroutine != null) StopWriting();
            _typingCoroutine = StartCoroutine(WriteText(text, onComplete));
        }

        private void StopWriting()
        {
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine); _typingCoroutine = null;
        }
        
        private void FinishWriting()
        {
            StopWriting();
            dialogueUI.SetDialogue(_currentLine);
            _isTyping = false;
        }
        
        private IEnumerator WriteText(string text, Action onComplete)
        {
            _isTyping = true;
            for(var i = 0; i<= text.Length; i++)
            {
                dialogueUI.SetDialogue(text.Substring(0, i));
                yield return new WaitForSeconds(textSpeed);
            }
            _isTyping = false;
            onComplete?.Invoke();
        }

        #endregion

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
            dialogueUI.HideCharacter();
            switch (tagKey)
            {
                case BackgroundTag:
                    if (int.TryParse(tagValue, out var bgIdx))
                    {
                        dialogueUI.SetBackground(inkData.backgroundImages[bgIdx]);
                    }
                    break;
                case CharacterNameTag:
                    dialogueUI.SetName(tagValue == MainCharacterTag ? _gameManager.saveData.playerName : tagValue);
                    break;
                case CharacterImageTag:
                    if (int.TryParse(tagValue, out var chaIdx))
                    {
                        dialogueUI.SetCharacter(inkData.characterImages[chaIdx]);
                    }
                    break;
                case NarrationTag:
                    dialogueUI.HideNameBox();
                    dialogueUI.HideCharacter();
                    break;
                case EndNarrationTag:
                    dialogueUI.ShowNameBox();
                    break;
                case SceneChangeTag:
                    _isSceneChanging = true;
                    transitionManager.LoadScene(tagValue, "Fade", 1f);
                    break;
            }
        }
    }
}
