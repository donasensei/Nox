using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private BoolDialog @bool;
    [SerializeField] private ErrorDialog @error;

    // Texts
    private const string NewGameText = "새로운 게임을 시작합니다.";
    private const string LoadGameText = "이전에 저장한 게임을 불러옵니다.";
    private const string QuitGameText = "게임의 설정을 변경합니다.";

    public void NewGame()
    {
        GameManager.Instance.state = GameManager.GameState.NewGame;
        SceneManager.Instance.LoadScene("SaveLoadMenu");
    }

    public void QuitGame()
    {
        @bool.SetInfoText(QuitGameText);
        @bool.Show();
    }

    public void LoadGame()
    {
        GameManager.Instance.state = GameManager.GameState.LoadGame;
        SceneManager.Instance.LoadScene("SaveLoadMenu");
    }

    public void Options()
    {
        Debug.Log("Option Ready Yet");
        @error.SetErrorText("옵션은 작업 중입니다.");
        @error.Show();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
