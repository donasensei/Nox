using UnityEngine;
using UnityEngine.UI;

public class ExploreButton : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {

    }

    private void CheckStates()
    {
        GameManager gameManager = GameManager.Instance;

        switch(gameManager.SaveData.dayNight)
        {
            case DayNight.Day:
                SceneManager.Instance.LoadScene(1);
                break;
            case DayNight.Night:
                SceneManager.Instance.LoadScene("Day");
                break;
        }
    }
}
