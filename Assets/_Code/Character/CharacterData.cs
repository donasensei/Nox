using System.Collections.Generic;
using System.Linq;
using _Code.Skill;
using UnityEngine;

namespace _Code.Character
{
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
        
        [Tooltip("���� �������� ����")]
        public List<float> activeBuffs = new();
        
        // HideInInspector
        [HideInInspector] public bool isAlive = true;
        [HideInInspector] public bool isSuppressed = false;
        public void UpdateStats()
        {
            characterStat.maxHealth = characterStat.CalculateMaxHealth(level, characterStat.vitality);
            characterStat.maxMana = characterStat.CalculateMaxMana(level,characterStat.magic, characterStat.blessing);
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
        }
        
        public void TakeHeal(int heal)
        {
            currentHealth += heal;
            if (currentHealth > characterStat.maxHealth)
            {
                currentHealth = characterStat.maxHealth;
            }
        }

        public void ApplyBuff(float percentage)
        {
            activeBuffs.Add(percentage);
        }
        
        public float GetDamageMultiplier()
        {
            var totalPercentage = activeBuffs.Sum();
            activeBuffs.Clear();
            
            return 1f + (totalPercentage / 100f);
        }
    }

    [System.Serializable]
    public struct CharacterStat
    {
        [HideInInspector] public int maxHealth;
        [HideInInspector] public int maxMana;

        public int strength; // ���ݷ� ����
        public int magic; // ���� ���ݷ� ����
        public int vitality; // ü�� ����
        public int blessing; // ȸ���� ����
        public int bonusStat;

        public int CalculateMaxHealth(int lv, int vit)
        {
            return Mathf.RoundToInt((vit * 6) + (lv * 6));
        }

        public int CalculateMaxMana(int lv, int mg, int bs)
        {
            return Mathf.RoundToInt((mg * 3) + (lv * 3) + (bs * 2));
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

        public void UpdateStats()
        {
            characterStat.maxHealth = characterStat.CalculateMaxHealth(level, characterStat.vitality);
            characterStat.maxMana = characterStat.CalculateMaxMana(level,characterStat.magic, characterStat.blessing);
        }
    }
}