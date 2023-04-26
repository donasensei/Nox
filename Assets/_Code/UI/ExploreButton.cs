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
        CheckStates();
    }

    private void CheckStates()
    {
        GameManager gameManager = GameManager.Instance;

        switch(gameManager.SaveData.dayNight)
        {
            case DayNight.Day:
                GameManager.Instance.SaveData.dayNight = DayNight.Night;
                CustomSceneManager.Instance.LoadScene("Intro01");
                break;
            case DayNight.Night:
                GameManager.Instance.SaveData.dayNight = DayNight.Day;
                CustomSceneManager.Instance.LoadScene("Daytime");
                break;
        }
    }
}
