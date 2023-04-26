using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/New Character")]
[System.Serializable]
public class CharacterData : ScriptableObject
{
    [Tooltip("캐릭터 이름")]
    public string characterName;

    [TextArea, Tooltip("캐릭터 설명")]
    public string characterDesc;

    [Tooltip("캐릭터 이미지")]
    public Sprite characterImage;

    [Tooltip("캐릭터 스탯")]
    public CharacterStat characterStat;

    [Tooltip("캐릭터 현재 스텟")]
    public int currentHealth;
    public int currentMana;

    [Tooltip("캐릭터 레벨")]
    public int level;

    [Tooltip("캐릭터 경험치")]
    public int experience;

    [Tooltip("캐릭터 스킬")]
    public List<SkillData> characterSkill = new();

    [Tooltip("기본 캐릭터 기능")]
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

    public int Strength; // 공격력 증가
    public int Magic; // 마법 공격력 증가
    public int Vitality; // 체력 증가
    public int Blessing; // 회복량 증가
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