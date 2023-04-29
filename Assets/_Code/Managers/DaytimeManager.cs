using _Code.Dialogue;
using _Code.UI;
using UnityEngine;

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
        private void Start()
        {

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
            CustomSceneManager.instance.LoadScene("CharacterEdit");
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
