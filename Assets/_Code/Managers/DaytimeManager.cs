using System;
using _Code.Dialogue;
using _Code.UI;
using UnityEngine;
using EasyTransition;

namespace _Code.Managers
{
    public class DaytimeManager : MonoBehaviour
    {
        [SerializeField] private ErrorDialog errorDialog;
        [SerializeField] private DialogueSystem diaUI;

        // Texts
        private const string SkillTreeText = "스킬 트리는 작업 중입니다.";
        private const string TownEditText = "마을 운영은 작업 중입니다.";

        [SerializeField] private CanvasGroup[] canvasGroups;

        private TransitionManager _transitionManager;

        private void Start()
        {
            _transitionManager = gameObject.AddComponent<TransitionManager>();
        }

        private void Update()
        {
            if(diaUI.gameObject.activeSelf)
            { 
                foreach (var group in canvasGroups)
                {
                    group.interactable = false;
                }
            }
            else
            {
                foreach (var group in canvasGroups)
                {
                    group.interactable = true;
                }
            }
        }

        public void CharacterEdit()
        {
            _transitionManager.LoadScene("CharacterEdit", "Fade", 1f);
        }

        public void SkillTree()
        {
            // Debug.Log("Skill Tree");
            errorDialog.SetErrorText(SkillTreeText);
            errorDialog.Show();
        }

        public void TownEdit()
        {
            // CustomSceneManager.Instance.LoadScene("BattleUI");

        }

        private void OnDialogHiddenHandler()
        {
            errorDialog.OnDialogHidden -= OnDialogHiddenHandler;
        }
    }
}
