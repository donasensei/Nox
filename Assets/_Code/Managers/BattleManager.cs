using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Code.Character;
using _Code.Skill;
using UnityEngine.Serialization;

namespace _Code.Managers
{
    public class BattleManager : MonoBehaviour
    {
        // Data
        public List<CharacterData> playerParty;
        public List<CharacterData> enemyParty;
        [SerializeField] private IEnumerable<CharacterData> enemyPartyData;

        // Action Queue
        private List<ScheduledAction> _scheduledActions;

        // Manager
        private GameManager _gameManager;
        
        // Constant
        private const float TurnBuffer = 0.5f;
        
        private void Start()
        {
            _scheduledActions = new List<ScheduledAction>();
            GetCharacterData();
        }

        #region 캐릭터 데이터

        private void GetCharacterData()
        {
            playerParty = _gameManager.ConvertToCharacterDataList(_gameManager.saveData.partyList);
            foreach (var enemy in enemyPartyData)
            {
                enemyParty.Add(enemy);
            }
        }

        #endregion
        
        // 이걸로 캐릭터가 사용할 스킬 스케줄링
        public void ScheduleSkill(CharacterData user, SkillData skill, CharacterData target, int time)
        {
            _scheduledActions.Add(new ScheduledAction(user, skill, target, time));
        }
        
        // 턴 시작 시 호출
        public void StartTurn()
        {
            StartCoroutine(ExecuteTurn());
        }

        // 턴 수행
        private IEnumerator ExecuteTurn()
        {
            var timer = 0f;
            while (_scheduledActions.Count > 0)
            {
                timer += Time.deltaTime;

                for (var i = _scheduledActions.Count - 1; i >= 0; i--)
                {
                    if (!(timer >= _scheduledActions[i].time)) continue;
                    if (_scheduledActions[i].user.currentHealth > 0)
                    {
                        _scheduledActions[i].skill.UseSkill(_scheduledActions[i].user, _scheduledActions[i].target,
                            _scheduledActions[i].time);
                    }

                    _scheduledActions.RemoveAt(i);
                }

                yield return new WaitForSeconds(TurnBuffer);

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
                return party[0].currentHealth <= 0;
            }
            return party.All(character => character.currentHealth <= 0);
        }

        private void HandleBattleOutcome(bool playerWon)
        {
            Debug.Log(playerWon ? "Player won" : "Player lost");
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
