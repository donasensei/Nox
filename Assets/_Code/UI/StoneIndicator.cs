using UnityEngine;
using UnityEngine.UI;

public class StoneIndicator : MonoBehaviour
{
    public Text text;
    private GameManager manager;

    private void Start()
    {
        manager = GameManager.Instance;
    }

    private void Update()
    {
        StoneCounter();
    }

    public void StoneCounter()
    {
        uint stoneCount = manager.SaveData.stones;
        text.text = stoneCount.ToString();
    }
}
