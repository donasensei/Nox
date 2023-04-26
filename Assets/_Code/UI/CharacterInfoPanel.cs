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
    [SerializeField] private Text characterVitalityText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text manaText;

    // Prefab
    [SerializeField] private SkillInfoPanel skillInfoPanel;

    // Buttons
    [SerializeField] private Button strPlus;
    [SerializeField] private Button strMinus;
    [SerializeField] private Button magPlus;
    [SerializeField] private Button magMinus;
    [SerializeField] private Button blessPlus;
    [SerializeField] private Button blessMinus;
    [SerializeField] private Button vitPlus;
    [SerializeField] private Button vitMinus;
    [SerializeField] private Button levelUpButton;


    // Image
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;

    private CharacterDataWrapper characterData;

    private void Awake()
    {
        strPlus.onClick.AddListener(() => IncreaseStat(ref characterData.characterStat.Strength, characterStrengthText));
        strMinus.onClick.AddListener(() => DecreaseStat(ref characterData.characterStat.Strength, characterStrengthText));
        magPlus.onClick.AddListener(() => IncreaseStat(ref characterData.characterStat.Magic, characterMagicText));
        magMinus.onClick.AddListener(() => DecreaseStat(ref characterData.characterStat.Magic, characterMagicText));
        blessPlus.onClick.AddListener(() => IncreaseStat(ref characterData.characterStat.Blessing, characterBlessText));
        blessMinus.onClick.AddListener(() => DecreaseStat(ref characterData.characterStat.Blessing, characterBlessText));
        vitPlus.onClick.AddListener(() => IncreaseStat(ref characterData.characterStat.Vitality, characterVitalityText));
        vitMinus.onClick.AddListener(() => DecreaseStat(ref characterData.characterStat.Vitality, characterVitalityText));

        levelUpButton.onClick.AddListener(LevelUpCharacter);
    }

    public void DisplayCharacterInfo(CharacterDataWrapper data)
    {
        characterData = data;

        // Basic Info
        characterNameText.text = characterData.characterName;
        characterLevelText.text = characterData.level.ToString();
        characterExpText.text = characterData.experience.ToString();
        characterStrengthText.text = characterData.characterStat.Strength.ToString();
        characterMagicText.text = characterData.characterStat.Magic.ToString();
        characterBlessText.text = characterData.characterStat.Blessing.ToString();
        characterVitalityText.text = characterData.characterStat.Vitality.ToString();

        // Health and Mana
        float healthPercent = (float)characterData.currentHealth / characterData.characterStat.MaxHealth;
        healthBar.fillAmount = healthPercent;
        healthText.text = characterData.currentHealth.ToString() + "/" + characterData.characterStat.MaxHealth.ToString();
        
        float manaPercent = (float)characterData.currentMana / characterData.characterStat.MaxMana;
        manaBar.fillAmount = manaPercent;
        manaText.text = characterData.currentMana.ToString() + "/" + characterData.characterStat.MaxMana.ToString();

        // Skill Field
        skillInfoPanel.SetupSkillToggles(characterData.characterSkill);
        levelUpButton.gameObject.SetActive(CanLevelUp());
        UpdateStatButtons();
    }

    private void UpdateStatButtons()
    {
        bool canIncreaseStat = characterData.characterStat.BonusStat > 0;

        strPlus.gameObject.SetActive(canIncreaseStat);
        magPlus.gameObject.SetActive(canIncreaseStat);
        blessPlus.gameObject.SetActive(canIncreaseStat);
        vitPlus.gameObject.SetActive(canIncreaseStat);
    }

    public void IncreaseStat(ref int stat, Text statText)
    {
        if(characterData.characterStat.BonusStat > 0)
        {
            stat++;
            characterData.characterStat.BonusStat--;
            statText.text = stat.ToString();
            UpdateCharacterData();

            levelUpButton.gameObject.SetActive(CanLevelUp());
        }
    }

    public void DecreaseStat(ref int stat, Text statText)
    {
        if (stat > 0)
        {
            stat--;
            statText.text = stat.ToString();
            UpdateCharacterData();
        }
    }

    private void UpdateCharacterData()
    {
        characterData.characterStat.Strength = int.Parse(characterStrengthText.text);
        characterData.characterStat.Magic = int.Parse(characterMagicText.text);
        characterData.characterStat.Blessing = int.Parse(characterBlessText.text);
        characterData.characterStat.Vitality = int.Parse(characterVitalityText.text);

        GameManager.Instance.UpdateCharacterData(characterData);

        UpdateStatButtons();
    }

    private bool CanLevelUp()
    {
        int requiredStones = CalculateRequiredStonesForLevelUp();
        return GameManager.Instance.SaveData.stones >= requiredStones;
    }

    private int CalculateRequiredStonesForLevelUp()
    {
        int baseStones = 2;
        int levelInterval = 3;
        int additionalStones = Mathf.FloorToInt((characterData.level - 1) / levelInterval);
        return baseStones * (int)Mathf.Pow(2, additionalStones);
    }

    private void LevelUpCharacter()
    {
        int requiredStones = CalculateRequiredStonesForLevelUp();
        if (CanLevelUp())
        {
            characterData.level++;
            characterData.characterStat.BonusStat += 3;
            GameManager.Instance.SaveData.stones -= (uint)requiredStones;

            characterLevelText.text = characterData.level.ToString();
            UpdateCharacterData();
        }

        // Recheck if there are enough stones for leveling up after leveling up
        levelUpButton.gameObject.SetActive(CanLevelUp());
    }
}