using System.Collections.Generic;
using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class ESCMenu : MonoBehaviour
    {
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private BoolDialog @bool;

        private const string quitText = "게임을 종료하시겠습니까?";

        private List<CanvasGroup> canvasGroups = new();

        private void Start()
        {
            /*saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);*/
            quitButton.onClick.AddListener(Quit);

            this.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            canvasGroups.AddRange(FindObjectsOfType<CanvasGroup>());

            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.interactable = false;
            }
        }

        private void OnDisable()
        {
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.interactable = true;
            }

            canvasGroups.Clear();
        }

        /*private void Save()
        {
            GameManager.instance.state = GameState.SaveGame;
            CustomSceneManager.instance.LoadScene("SaveLoadMenu");
        }

        private void Load()
        {
            GameManager.instance.state = GameState.LoadGame;
            CustomSceneManager.instance.LoadScene("SaveLoadMenu");
        }*/

        private void Quit()
        {
            @bool.SetInfoText(quitText);
            @bool.Show();
            @bool.OnConfirm += Application.Quit;
            @bool.OnCancel += () => { @bool.Hide(); };
        }
    }
}
