using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    [Tooltip("��ų �̸�")]
    public string skillName;

    [Tooltip("��ų �̹���")]
    public Sprite skillImage;

    [Tooltip("��ų ����"), TextArea]
    public string skillDesc;

    [Tooltip("��ų Ÿ��")]
    public SkillType skillType;

    [Tooltip("��ų �ڽ�Ʈ")]
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