using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private ErrorDialog errorDialog;

    // Texts
    private const string SkillTreeText = "스킬 트리는 작업 중입니다.";
    private const string TownEditText = "마을 운영은 작업 중입니다.";

    public void SkillTree()
    {
        // Debug.Log("Skill Tree");
        errorDialog.SetErrorText(SkillTreeText);
        errorDialog.Show();
    }

    public void TownEdit()
    {
        // Debug.Log("Town Edit");
        errorDialog.SetErrorText(TownEditText);
        errorDialog.Show();
    }

    private void OnDialogHiddenHandler()
    {
        errorDialog.OnDialogHidden -= OnDialogHiddenHandler;
    }
}
