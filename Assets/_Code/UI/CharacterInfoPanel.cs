using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    // Texts
    [SerializeField] private Text characterNameText;
    [SerializeField] private Text characterLevelText;
    [SerializeField] private Text characterExpText;
    [SerializeField] private Text characterStrengthText;
    [SerializeField] private Text characterMagicText;
    [SerializeField] private Text characterBlessText;

    [SerializeField] private SkillInfoPanel skillInfoPanel;

    // Image
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;

    private CharacterDataWrapper characterDataWrapper;

    public void DisplayCharacterInfo(CharacterDataWrapper characterData)
    {
        characterDataWrapper = characterData;

        characterNameText.text = characterData.characterName;
        characterLevelText.text = characterData.level.ToString();
        characterExpText.text = characterData.experience.ToString();
        characterStrengthText.text = characterData.characterStat.Strength.ToString();
        characterMagicText.text = characterData.characterStat.Magic.ToString();
        characterBlessText.text = characterData.characterStat.Blessing.ToString();

        float healthPercent = (float)characterData.currentHealth / characterData.characterStat.MaxHealth;
        healthBar.fillAmount = healthPercent;
        float manaPercent = (float)characterData.currentMana / characterData.characterStat.MaxMana;
        manaBar.fillAmount = manaPercent;

        skillInfoPanel.SetupSkillToggles(characterData.characterSkill);
    }
}