using UnityEngine;

public class DaytimeManager : MonoBehaviour
{
    [SerializeField] private ErrorDialog errorDialog;

    public void SkillTree()
    {
        Debug.Log("Skill Tree");
        errorDialog.errorText.text = "스킬 트리는 작업 중입니다.";
        errorDialog.Show();
    }

    public void TownEdit()
    {
        Debug.Log("Town Edit");
        errorDialog.errorText.text = "마을 운영은 작업 중입니다.";
        errorDialog.Show();
    }

    private void OnDialogHiddenHandler()
    {
        errorDialog.OnDialogHidden -= OnDialogHiddenHandler;
    }
}
