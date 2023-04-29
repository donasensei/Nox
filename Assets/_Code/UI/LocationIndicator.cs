using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class LocationIndicator : MonoBehaviour
    {
        // UI
        [SerializeField] private Text locationText;
        [SerializeField] private Text percentage;
        
        // Manager
        private GameManager _gameManager; 

        private void Start()
        {
            locationText.text = _gameManager.saveData.currentLocation;
            percentage.text = "0%";
        }

        private void Update()
        {
            // locationText.text =  GameManager.instance.saveData.currentLocation;
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
}
