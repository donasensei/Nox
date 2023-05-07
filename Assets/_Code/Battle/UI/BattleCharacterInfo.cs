using _Code.Character;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Battle.UI
{
    public class BattleCharacterInfo : MonoBehaviour
    {
        [SerializeField] private Text characterName;
        [SerializeField] private Text characterHp;
        [SerializeField] private Text characterMp;
        [SerializeField] private Image healthBar;
        [SerializeField] private Image manaBar;
        [SerializeField] private BattleSkillInfoPanel skillInfoPanel;
        
        public CharacterData characterData { get; private set; }
        
        public void UpdateInfo(CharacterData data)
        {
            characterData = data; 
            characterName.text = data.characterName;

            var healthPercent = (float)data.currentHealth / data.characterStat.maxHealth;
            healthBar.fillAmount = healthPercent;
            characterHp.text = data.currentHealth.ToString() + "/" + data.characterStat.maxHealth.ToString();

            var manaPercent = (float)data.currentMana / data.characterStat.maxMana;
            manaBar.fillAmount = manaPercent;
            characterMp.text = data.currentMana.ToString() + "/" + data.characterStat.maxMana.ToString();
            
            // Skill Field
            skillInfoPanel.SetupSkillToggles(data.characterSkill);
        }
    }
}
