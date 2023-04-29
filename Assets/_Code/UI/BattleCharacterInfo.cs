using _Code.Character;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class BattleCharacterInfo : MonoBehaviour
    {
        [SerializeField] private Text characterName;
        [SerializeField] private Text characterHP;
        [SerializeField] private Text characterMP;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image manaBar;

        public void UpdateInfo(CharacterData data)
        {
            characterName.text = data.characterName;

            float healthPercent = (float)data.currentHealth / data.characterStat.maxHealth;
            healthBar.fillAmount = healthPercent;
            characterHP.text = data.currentHealth.ToString() + "/" + data.characterStat.maxHealth.ToString();

            float manaPercent = (float)data.currentMana / data.characterStat.maxMana;
            manaBar.fillAmount = manaPercent;
            characterMP.text = data.currentMana.ToString() + "/" + data.characterStat.maxMana.ToString();
        }
    }
}
