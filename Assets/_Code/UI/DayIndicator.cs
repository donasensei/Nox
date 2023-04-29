using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class DayIndicator : MonoBehaviour
    {
        public Text text;
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.instance;
        }

        private void Update()
        {
            DayCounter();
        }

        private void DayCounter()
        {
            var dayCount = _gameManager.saveData.currentDay;
            text.text = "Day\n" + dayCount;
        }
    }
}
