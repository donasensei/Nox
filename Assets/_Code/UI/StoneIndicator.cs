using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class StoneIndicator : MonoBehaviour
    {
        [SerializeField] private Text text;
        private GameManager _gameManager;
        
        private void Start()
        {
            _gameManager = GameManager.instance;
        }
        
        private void Update()
        {
            StoneCounter();
        }

        private void StoneCounter()
        {
            var stoneCount = _gameManager.saveData.stones;
            text.text = stoneCount.ToString();
        }
    }
}
