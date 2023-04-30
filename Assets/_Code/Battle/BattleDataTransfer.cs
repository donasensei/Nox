using System.Collections.Generic;
using _Code.Character;
using UnityEngine;

namespace _Code.Battle
{
    public class BattleDataTransfer : MonoBehaviour
    {
        public static BattleDataTransfer Instance;

        public List<CharacterData> enemyCharacters;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}