using _Code.Character;
using UnityEngine;

namespace _Code.Skill
{
    [CreateAssetMenu(fileName = "New Heal Skill", menuName = "Skill/New Heal Skill")]
    public class HealSkill : SkillData
    {
        [Tooltip("스킬 회복력")]
        public int healAmount;
        
        private HealSkill() 
        {
            skillType = SkillType.Heal;
        }
        
        // Methods
        public override void UseSkill(CharacterData user, CharacterData target, int time)
        {
            Debug.Log("Use Skill" + this.skillName);
            var timeMultiplier = Mathf.Lerp(1f, 2.5f, (time - 1f) / 9f);
            var healMultiplier = Mathf.Lerp(1f, 2.5f, (time - 1f) / 9f);
            var heal = (healAmount * healMultiplier * timeMultiplier) * (user.characterStat.blessing * 0.3f);
            
            target.TakeHeal((int)heal);
        }
    }
}