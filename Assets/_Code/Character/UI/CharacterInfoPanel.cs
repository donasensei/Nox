using _Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Character.UI
{
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

        // Data Container
        private CharacterDataWrapper _characterData;
        
        // Manager
        private GameManager _gameManager;

        private void Awake()
        {
            strPlus.onClick.AddListener(() => IncreaseStat(ref _characterData.characterStat.strength, characterStrengthText));
            strMinus.onClick.AddListener(() => DecreaseStat(ref _characterData.characterStat.strength, characterStrengthText));
            magPlus.onClick.AddListener(() => IncreaseStat(ref _characterData.characterStat.magic, characterMagicText));
            magMinus.onClick.AddListener(() => DecreaseStat(ref _characterData.characterStat.magic, characterMagicText));
            blessPlus.onClick.AddListener(() => IncreaseStat(ref _characterData.characterStat.blessing, characterBlessText));
            blessMinus.onClick.AddListener(() => DecreaseStat(ref _characterData.characterStat.blessing, characterBlessText));
            vitPlus.onClick.AddListener(() => IncreaseStat(ref _characterData.characterStat.vitality, characterVitalityText));
            vitMinus.onClick.AddListener(() => DecreaseStat(ref _characterData.characterStat.vitality, characterVitalityText));

            levelUpButton.onClick.AddListener(LevelUpCharacter);
        }

        public void DisplayCharacterInfo(CharacterDataWrapper data)
        {
            _characterData = data;

            // Basic Info
            characterNameText.text = _characterData.characterName;
            characterLevelText.text = _characterData.level.ToString();
            characterExpText.text = _characterData.experience.ToString();
            characterStrengthText.text = _characterData.characterStat.strength.ToString();
            characterMagicText.text = _characterData.characterStat.magic.ToString();
            characterBlessText.text = _characterData.characterStat.blessing.ToString();
            characterVitalityText.text = _characterData.characterStat.vitality.ToString();

            // Health and Mana
            var healthPercent = (float)_characterData.currentHealth / _characterData.characterStat.maxHealth;
            healthBar.fillAmount = healthPercent;
            healthText.text = _characterData.currentHealth.ToString() + "/" + _characterData.characterStat.maxHealth.ToString();
        
            var manaPercent = (float)_characterData.currentMana / _characterData.characterStat.maxMana;
            manaBar.fillAmount = manaPercent;
            manaText.text = _characterData.currentMana.ToString() + "/" + _characterData.characterStat.maxMana.ToString();

            // Skill Field
            skillInfoPanel.SetupSkillToggles(_characterData.characterSkill);
            levelUpButton.gameObject.SetActive(CanLevelUp());
            UpdateStatButtons();
        }

        private void UpdateStatButtons()
        {
            var canIncreaseStat = _characterData.characterStat.bonusStat > 0;

            strPlus.gameObject.SetActive(canIncreaseStat);
            magPlus.gameObject.SetActive(canIncreaseStat);
            blessPlus.gameObject.SetActive(canIncreaseStat);
            vitPlus.gameObject.SetActive(canIncreaseStat);
        }

        private void IncreaseStat(ref int stat, Text statText)
        {
            if (_characterData.characterStat.bonusStat <= 0) return;
            stat++;
            _characterData.characterStat.bonusStat--;
            statText.text = stat.ToString();
            UpdateCharacterData();

            levelUpButton.gameObject.SetActive(CanLevelUp());
        }

        private void DecreaseStat(ref int stat, Text statText)
        {
            if (stat <= 0) return;
            stat--;
            statText.text = stat.ToString();
            UpdateCharacterData();
        }

        private void UpdateCharacterData()
        {
            _characterData.characterStat.strength = int.Parse(characterStrengthText.text);
            _characterData.characterStat.magic = int.Parse(characterMagicText.text);
            _characterData.characterStat.blessing = int.Parse(characterBlessText.text);
            _characterData.characterStat.vitality = int.Parse(characterVitalityText.text);

            GameManager.instance.UpdateCharacterData(_characterData);

            UpdateStatButtons();
        }

        private bool CanLevelUp()
        {
            var requiredStones = CalculateRequiredStonesForLevelUp();
            return _gameManager.saveData.stones >= requiredStones;
        }

        private int CalculateRequiredStonesForLevelUp()
        {
            const int baseStones = 2;
            const int levelInterval = 3;
            // ReSharper disable once PossibleLossOfFraction
            var additionalStones = Mathf.FloorToInt((_characterData.level - 1) / levelInterval);
            return baseStones * (int)Mathf.Pow(2, additionalStones);
        }

        private void LevelUpCharacter()
        {
            var requiredStones = CalculateRequiredStonesForLevelUp();
            if (CanLevelUp())
            {
                _characterData.level++;
                _characterData.characterStat.bonusStat += 3;
                _gameManager.saveData.stones -= (uint)requiredStones;

                characterLevelText.text = _characterData.level.ToString();
                UpdateCharacterData();
            }

            // Check Level Up Availability
            levelUpButton.gameObject.SetActive(CanLevelUp());
        }
    }
}