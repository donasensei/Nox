using UnityEngine;
using UnityEngine.UI;

public class LocationIndicator : MonoBehaviour
{
    [SerializeField] private Text locationText;
    [SerializeField] private Text percentage;

    private void Start()
    {
        locationText.text = GameManager.Instance.SaveData.currentLocation;
        percentage.text = "0%";
    }

    private void Update()
    {
        locationText.text =  GameManager.Instance.SaveData.currentLocation;
    }

    public void SetPercentage(int value)
    {
        percentage.text = value.ToString() + "%";
    }

    public void SetLocationText(string location)
    {
        locationText.text = location;
    }
}
