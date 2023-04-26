using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    [Tooltip("스킬 이름")]
    public string skillName;

    [Tooltip("스킬 이미지")]
    public Sprite skillImage;

    [Tooltip("스킬 설명"), TextArea]
    public string skillDesc;

    [Tooltip("스킬 타입")]
    public SkillType skillType;

    [Tooltip("스킬 코스트")]
    public int skillCost;

    // Methods
    public virtual void UseSkill(CharacterData user, CharacterData target)
    {
        Debug.Log("Use Skill" + this.skillName);
    }
}

public enum SkillType
{
    None,
    Attack,
    Magic, 
    Heal,
    Buff
}