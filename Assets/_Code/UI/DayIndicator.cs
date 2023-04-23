using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayIndicator : MonoBehaviour
{
    public Text text;
    private GameManager manager;

    private void Start()
    {
        manager = GameManager.Instance;
    }

    private void Update()
    {
        DayCounter();
    }

    public void DayCounter()
    {
        uint dayCount = manager.SaveData.currentDay;
        text.text = "Day\n" + dayCount.ToString();
    }
}
