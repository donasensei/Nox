using _Code.Character;
using UnityEngine;

namespace _Code.Skill
{
    [CreateAssetMenu(fileName = "New MAgic Skill", menuName = "Skill/New Magic Skill")]
    public class MagicSkill : SkillData
    {
        [Tooltip("스킬 공격력")]
        public int skillDamage;

        private MagicSkill() 
        {
            skillType = SkillType.Magic;
        }

        // Methods
        public override void UseSkill(CharacterData user, CharacterData target, int time)
        {
            Debug.Log("Use Skill" + this.skillName);
            var damageMultiplier = Mathf.Lerp(1f, 2.5f, (time - 1f) / 9f);
            var damage = user.characterStat.magic * damageMultiplier;
                
            target.TakeDamage((int)damage);
        }
    }
}