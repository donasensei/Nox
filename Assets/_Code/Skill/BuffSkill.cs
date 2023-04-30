using _Code.Character;
using UnityEngine;

namespace _Code.Skill
{
    [CreateAssetMenu(fileName = "New Buff Skill", menuName = "Skill/New Buff Skill")]
    public class BuffSkill : SkillData
    {
        [Tooltip("효과 증가 퍼센트")]
        [Range(0, 100)]
        public float buffPercentage;
        
        private BuffSkill()
        {
            skillType = SkillType.Buff;
        }

        // Methods
        public override void UseSkill(CharacterData user, CharacterData target, int time)
        {
            Debug.Log("Use Skill" + this.skillName);

            var timeMultiplier = Mathf.Lerp(0.0f, 20.0f, (time - 1f) / 9f);
            target.ApplyBuff(buffPercentage + timeMultiplier);
        }
    }
}