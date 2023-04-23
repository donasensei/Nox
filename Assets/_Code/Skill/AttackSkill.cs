using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Skill", menuName = "Skill/New Attack Skill")]
public class AttackSkill : SkillData
{
    [Tooltip("스킬 공격력")]
    public int skillDamage;

    [Tooltip("보너스 스킬 효과")]
    public AttackSkillBonus attackSkillBonus;

    AttackSkill() 
    {
        skillType = SkillType.Attack;
    }

    // Methods
    public override void UseSkill(CharacterData user, CharacterData target)
    {
        Debug.Log("Use Skill" + this.skillName);

        int damage = skillDamage;
        damage = Mathf.RoundToInt((user.characterStat.Magic * 0.3f) * damage);
        target.currentHealth -= damage;
    }

    private void IncreaseStrength(CharacterData user)
    {
        user.characterStat.Strength += 1;
    }

    private void IncreaseHealth(CharacterData user)
    {
        user.characterStat.Vitality += 1;
    }
}

public enum AttackSkillBonus
{
    None,
    IncreaseStrength,
    IncreaseHealth
}