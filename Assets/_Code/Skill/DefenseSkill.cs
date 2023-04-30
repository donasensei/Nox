using UnityEngine;
using _Code.Character;

namespace _Code.Skill
{
    [CreateAssetMenu(fileName = "New Defense Skill", menuName = "Skill/New Defense Skill")]
    public class DefenseSkill : SkillData
    {
        private DefenseSkill()
        {
            skillType = SkillType.Defense;
        }
        
        public override void UseSkill(CharacterData user, CharacterData target, int time)
        {
            Debug.Log("Use Skill" + this.skillName);
            if (skillType == SkillType.Defense)
            {
                user.isDefending = true;
            }
        }
    }
}