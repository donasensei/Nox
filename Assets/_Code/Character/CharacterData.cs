using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/New Character")]
[System.Serializable]
public class CharacterData : ScriptableObject
{
    [Tooltip("ĳ���� �̸�")]
    public string characterName;

    [TextArea, Tooltip("ĳ���� ����")]
    public string characterDesc;

    [Tooltip("ĳ���� �̹���")]
    public Sprite characterImage;

    [Tooltip("ĳ���� ����")]
    public CharacterStat characterStat;

    [Tooltip("ĳ���� ���� ����")]
    public int currentHealth;
    public int currentMana;

    [Tooltip("ĳ���� ����")]
    public int level;

    [Tooltip("ĳ���� ����ġ")]
    public int experience;

    [Tooltip("ĳ���� ��ų")]
    public List<SkillData> characterSkill = new();

    [Tooltip("�⺻ ĳ���� ���")]
    public List<SkillData> defaultSkill = new();

    public void UpdateMaxHealthAndMana()
    {
        characterStat.MaxHealth = characterStat.CalculateMaxHealth(level, characterStat.Vitality);
        characterStat.MaxMana = characterStat.CalculateMaxMana(level,characterStat.Magic, characterStat.Blessing);
    }
}

[System.Serializable]
public struct CharacterStat
{
    [HideInInspector] public int MaxHealth;
    [HideInInspector] public int MaxMana;

    public int Strength; // ���ݷ� ����
    public int Magic; // ���� ���ݷ� ����
    public int Vitality; // ü�� ����
    public int Blessing; // ȸ���� ����
    public int BonusStat;

    public int CalculateMaxHealth(int level, int vitality)
    {
        return Mathf.RoundToInt((vitality * 6) + (level * 6));
    }

    public int CalculateMaxMana(int level, int magic, int blessing)
    {
        return Mathf.RoundToInt((magic * 3) + (level * 3) + (blessing * 2));
    }
}

[System.Serializable]
public class CharacterDataWrapper
{
    public string characterName;
    public string characterDesc;
    public Sprite characterImage;
    public CharacterStat characterStat;
    public int currentHealth;
    public int currentMana;
    public int level;
    public int experience;
    public List<SkillData> characterSkill;
    public List<SkillData> defaultSkill;

    public CharacterDataWrapper(CharacterData characterData)
    {
        characterName = characterData.characterName;
        characterDesc = characterData.characterDesc;
        characterImage = characterData.characterImage;
        characterStat = characterData.characterStat;
        currentHealth = characterData.currentHealth;
        currentMana = characterData.currentMana;
        level = characterData.level;
        experience = characterData.experience;
        characterSkill = characterData.characterSkill;
        defaultSkill = characterData.defaultSkill;
    }

    public void UpdateMaxHealthAndMana()
    {
        characterStat.MaxHealth = characterStat.CalculateMaxHealth(level, characterStat.Vitality);
        characterStat.MaxMana = characterStat.CalculateMaxMana(level, characterStat.Magic, characterStat.Blessing);
    }
}