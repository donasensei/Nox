using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Code.Battle.UI;
using UnityEngine;
using _Code.Character;
using _Code.Managers;
using _Code.Skill;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Code.Battle
{
    public class BattleManager : MonoBehaviour
    {
        // Data
        [SerializeField] private List<CharacterData> playerParty;
        [SerializeField] private List<CharacterData> enemyParty;
        [SerializeField] private List<CharacterData> enemyPartyData;

        // Action Queue
        private List<ScheduledAction> _scheduledActions;

        // Manager
        private GameManager _gameManager;
        
        // Constant
        private const float TurnBuffer = 0.5f;
        
        // UI
        public BattleCharacterInfo characterInfo;
        public SkillToggle skillToggle;
        public Transform contentTransform;
        public BattleCharacterButton characterButtonPrefab;
        public ToggleGroup characterToggleGroup;
        public TMP_Dropdown enemyDropdown;
        public TMP_Dropdown timeDropdown;
        public BattleSkillInfoPanel battleSkillInfoPanel;
        public Button startTurnButton;
        
        private readonly List<BattleCharacterButton> _characterButtons = new();
        
        // Constant 
        private const int MaxTurns = 10;
        
        // Local Variables
        private CharacterData _selectedEnemy;
        private int _selectedTime;
        private SkillData _selectedSkill;
        
        private void Start()
        {
            _gameManager = GameManager.instance; 
            _scheduledActions = new List<ScheduledAction>();
            GetCharacterData();
            
            // UI
            PopulateCharacterList();
            PopulateTimeDropdown();
            PopulateEnemyDropdown();
            enemyDropdown.onValueChanged.AddListener(delegate { OnEnemyCharacterDropdownValueChanged(); });
            timeDropdown.onValueChanged.AddListener(delegate { OnTimeDropdownValueChanged(); });
            battleSkillInfoPanel.OnSkillSelected += OnSkillSelected;
            battleSkillInfoPanel.OnSkillSelected += OnSkillSelected;
            startTurnButton.onClick.AddListener(StartTurn);
            StartCoroutine(DisplayFirstCharacterInfo());
            startTurnButton.interactable = false;
        }

        private void Update()
        {
            if (playerParty.Count == _scheduledActions.Count)
            {
                startTurnButton.interactable = true;
            }
        }

        #region 캐릭터 데이터

        private void GetCharacterData()
        {
            playerParty = GameManager.ConvertToCharacterDataList(_gameManager.saveData.partyList);
            foreach (var enemy in enemyPartyData)
            {
                enemyParty.Add(enemy);
            }
        }

        #endregion

        #region UI 관련

        private void PopulateCharacterList()
        {
            foreach (var character in playerParty)
            {
                CreateCharacterButton(character);
            }
        }

        private void CreateCharacterButton(CharacterData character)
        {
            var button = Instantiate(characterButtonPrefab, contentTransform);
            button.Initialize(character, characterInfo);
            button.GetComponent<Toggle>().group = characterToggleGroup;
            button.OnCharacterSelected += OnCharacterButtonSelected;
            _characterButtons.Add(button);
        }
        
        private IEnumerator DisplayFirstCharacterInfo()
        {
            yield return new WaitForEndOfFrame();

            if (_characterButtons.Count > 0)
            {
                characterInfo.UpdateInfo(_characterButtons[0].characterData);
            }
        }
        
        private void PopulateEnemyDropdown()
        {
            enemyDropdown.ClearOptions();
            var enemyOptions = enemyParty.Select(enemy => new TMP_Dropdown.OptionData(enemy.characterName)).ToList();
            
            enemyDropdown.AddOptions(enemyOptions);
            enemyDropdown.onValueChanged.AddListener(delegate { OnEnemyCharacterDropdownValueChanged(); });
        }
        
        private void PopulateTimeDropdown()
        {
            timeDropdown.ClearOptions();
            var timeOptions = new List<TMP_Dropdown.OptionData>();
            for (var i = 1; i <= MaxTurns; i++)
            {
                timeOptions.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }

            timeDropdown.AddOptions(timeOptions);
            timeDropdown.onValueChanged.AddListener(delegate { OnTimeDropdownValueChanged(); });
        }
        
        #endregion

        #region OnEvent 관련

        private void OnEnemyCharacterDropdownValueChanged()
        {
            _selectedEnemy = enemyParty[enemyDropdown.value];
            UpdateScheduledSkill();
        }

        private void OnTimeDropdownValueChanged()
        {
            _selectedTime = timeDropdown.value + 1;
            UpdateScheduledSkill();
        }
        
        private void OnSkillSelected(SkillData skill)
        {
            _selectedSkill = skill;
            UpdateScheduledSkill();
        }
        
        private void OnCharacterButtonSelected(CharacterData characterData)
        {
            characterInfo.UpdateInfo(characterData);
            UpdateScheduledSkill();
        }


        #endregion
        
        // 이걸로 캐릭터가 사용할 스킬 스케줄링
        private void ScheduleSkill(CharacterData user, SkillData skill, CharacterData target, int time)
        {
            var existingScheduledAction = _scheduledActions.Find(action => action.user == user);
            if (existingScheduledAction != null)
            {
                RefundSkillCost(existingScheduledAction.user, existingScheduledAction.skill);
                _scheduledActions.Remove(existingScheduledAction);
            }
            
            var canUseSkill = false;
            switch (skill.skillType)
            {
                case SkillType.Attack:
                    if (user.currentHealth > skill.skillCost)
                    {
                        user.currentHealth -= skill.skillCost;
                        canUseSkill = true;
                    }
                    break;
                case SkillType.Magic:
                case SkillType.Buff:
                case SkillType.Heal:
                    if (user.currentMana >= skill.skillCost)
                    {
                        user.currentMana -= skill.skillCost;
                        canUseSkill = true;
                    }
                    break;
            }

            if (canUseSkill)
            {
                _scheduledActions.Add(new ScheduledAction(user, skill, target, time));
            }
            else
            {
                Debug.LogWarning("Not enough resources for " + user.characterName + " to use " + skill.skillName);
            }
        }
        
        // 스킬 소비량 환불
        private void RefundSkillCost(CharacterData user, SkillData skill)
        {
            switch (skill.skillType)
            {
                case SkillType.Attack:
                    user.currentHealth += skill.skillCost;
                    break;
                case SkillType.Magic:
                case SkillType.Buff:
                case SkillType.Heal:
                    user.currentMana += skill.skillCost;
                    break;
            }
        }

        private void UpdateScheduledSkill()
        {
            var user = characterInfo.characterData;
            var target = _selectedEnemy;
            var time = _selectedTime;
            var skill = _selectedSkill;

            if (user == null || target == null || skill == null) return;
            ScheduleSkill(user, skill, target, time);
            characterInfo.UpdateInfo(user);
        }

        // 턴 시작 시 호출
        private void StartTurn()
        {
            EnemyAI();
            Debug.Log("Enemy AI Finished");
            StartCoroutine(ExecuteTurn());
        }

        // 턴 수행
        private IEnumerator ExecuteTurn()
        {
            var timer = 0f;
            while (_scheduledActions.Count > 0)
            {
                timer += Time.deltaTime;
                Debug.Log(timer);

                for (var i = _scheduledActions.Count - 1; i >= 0; i--)
                {
                    if (!(timer >= _scheduledActions[i].time)) continue;
                    if (_scheduledActions[i].user.currentHealth > 0)
                    {
                        Debug.Log(_scheduledActions[i].user.characterName + " used " + _scheduledActions[i].skill.skillName);
                        _scheduledActions[i].skill.UseSkill(_scheduledActions[i].user, _scheduledActions[i].target,
                            _scheduledActions[i].time);
                    }
                    _scheduledActions.RemoveAt(i);
                }

                yield return new WaitForSeconds(1f);

                UpdateSuppressedState(playerParty);
                UpdateSuppressedState(enemyParty);

                if (IsPartyDefeated(playerParty, true))
                {
                    HandleBattleOutcome(false);
                    break;
                }
                else if (IsPartyDefeated(enemyParty))
                {
                    HandleBattleOutcome(true);
                    break;
                }
            }
            
            UpdateAliveState(playerParty);
            UpdateAliveState(enemyParty);
            
            ResetDefendingState(playerParty);
            ResetDefendingState(enemyParty);
        }

        #region 전투 상황 측정용
        private static void UpdateSuppressedState(IEnumerable<CharacterData> party)
        {
            foreach (var character in party.Where(character => character.currentHealth <= 0))
            {
                character.isSuppressed = true;
            }
        }

        private static void UpdateAliveState(IEnumerable<CharacterData> party)
        {
            foreach (var character in party.Where(character => character.currentHealth <= 0 && !character.isSuppressed))
            {
                character.isAlive = false;
            }
        }

        private static bool IsPartyDefeated(IReadOnlyList<CharacterData> party, bool isPlayerParty = false)
        {
            if (isPlayerParty)
            {
                return !party[0].isAlive;
            }
            return party.All(character => !character.isAlive);
        }


        private void HandleBattleOutcome(bool playerWon)
        {
            Debug.Log(playerWon ? "Player won" : "Player lost");
        }
        
        private static void ResetDefendingState(IEnumerable<CharacterData> party)
        {
            foreach (var character in party)
            {
                character.isDefending = false;
            }
        }

        #endregion

        #region 적 관련

        private void EnemyAI()
        {
            foreach (var enemy in enemyParty)
            {
                if (enemy.characterSkill.Count <= 0) continue;
                var randomSkillIndex = Random.Range(0, enemy.characterSkill.Count);
                var randomSkill = enemy.characterSkill[randomSkillIndex];

                var randomPlayerIndex = Random.Range(0, playerParty.Count);
                var randomPlayer = playerParty[randomPlayerIndex];

                var randomTime = Random.Range(1, MaxTurns + 1);

                ScheduleSkill(enemy, randomSkill, randomPlayer, randomTime);
            }
        }


        #endregion
    }

    public class ScheduledAction
    {
        public CharacterData user { get; }
        public SkillData skill { get; }
        public CharacterData target { get; }
        public int time { get; }

        public ScheduledAction(CharacterData user, SkillData skill, CharacterData target, int time)
        {
            this.user = user;
            this.skill = skill;
            this.target = target;
            this.time = time;
        }
    }
}
