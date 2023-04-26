using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

        float healthPercent = (float)data.currentHealth / data.characterStat.MaxHealth;
        healthBar.fillAmount = healthPercent;
        characterHP.text = data.currentHealth.ToString() + "/" + data.characterStat.MaxHealth.ToString();

        float manaPercent = (float)data.currentMana / data.characterStat.MaxMana;
        manaBar.fillAmount = manaPercent;
        characterMP.text = data.currentMana.ToString() + "/" + data.characterStat.MaxMana.ToString();
    }
}
