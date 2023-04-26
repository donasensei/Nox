using Ink.Parsed;
using UnityEngine;

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
        GameManager.Instance.state = GameManager.GameState.DayTime;
        GameManager.Instance.SaveData.currentLocation = "도적의 거점";
        if(!GameManager.Instance.SaveData.flags.isTutorialDone)
        {
            diaUI.StartStoryFirst = true;
        }    
        else
        {
            diaUI.StartStoryFirst = false;
        }
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
        CustomSceneManager.Instance.LoadScene("CharacterEdit");
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
        GameManager.Instance.state = GameManager.GameState.SaveGame;
        CustomSceneManager.Instance.LoadScene("SaveLoadMenu");
    }

    private void OnDialogHiddenHandler()
    {
        errorDialog.OnDialogHidden -= OnDialogHiddenHandler;
    }
}
