using System;
using _Code.UI;
using UnityEngine;

namespace _Code.Managers
{
    public class TitleScreenManager : MonoBehaviour
    {
        // UI
        [SerializeField] private BoolDialog @bool;
        [SerializeField] private ErrorDialog @error;
        [SerializeField] private SaveLoadManager saveLoadManager;

        // Texts
        private const string QuitGameText = "게임을 종료합니다.";
        
        // Manager
        private GameManager _gameManager;
        private CustomSceneManager _customSceneManager;

        private void Start()
        {
            _gameManager = GameManager.instance;
            _customSceneManager = CustomSceneManager.instance;
        }

        public void NewGame()
        {
            _gameManager.NewGame();
        }

        public void LoadGame()
        {
            _gameManager.state = GameState.Load;
            saveLoadManager.Show();
        }

        public void QuitGame()
        {
            @bool.SetInfoText(QuitGameText);
            @bool.Show();
            @bool.OnConfirm += Application.Quit;
            @bool.OnCancel += () => { @bool.Hide(); };
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
}
