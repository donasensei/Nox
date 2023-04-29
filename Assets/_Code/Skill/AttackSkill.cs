using _Code.Character;
using UnityEngine;

namespace _Code.Skill
{
    [CreateAssetMenu(fileName = "New Attack Skill", menuName = "Skill/New Attack Skill")]
    public class AttackSkill : SkillData
    {
        [Tooltip("스킬 공격력")]
        public int skillDamage;

        private AttackSkill() 
        {
            skillType = SkillType.Attack;
        }

        // Methods
        public override void UseSkill(CharacterData user, CharacterData target, int time)
        {
            Debug.Log("Use Skill" + this.skillName);
            
            var timeMultiplier = Mathf.Lerp(1f, 2.5f, (time - 1f) / 9f);
            var damageMultiplier = user.GetDamageMultiplier();
            var damage = user.characterStat.strength * damageMultiplier * timeMultiplier;
            
            target.TakeDamage((int)damage);
        }
    }
}