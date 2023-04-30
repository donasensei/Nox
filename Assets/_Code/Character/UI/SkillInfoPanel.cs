using System.Collections.Generic;
using _Code.Skill;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class SkillInfoPanel : MonoBehaviour
    {
        [SerializeField] private Text skillNameText;
        [SerializeField] private List<Toggle> skillToggles;
        [SerializeField] private ToggleGroup skillToggleGroup;
        [SerializeField] private Text skillDescText;
        [SerializeField] private Text skillTypeText;
        [SerializeField] private Text skillCostText;

        private void DisplaySkillInfo(SkillData skillData)
        {
            skillNameText.text = skillData.skillName;
            skillDescText.text = skillData.skillDesc;
            skillTypeText.text = skillData.skillType.ToString();
            skillCostText.text = skillData.skillCost.ToString();
        }

        public void SetupSkillToggles(List<SkillData> skills)
        {
            for (var i = 0; i < skillToggles.Count; i++)
            {
                if (i < skills.Count)
                {
                    skillToggles[i].group = skillToggleGroup;
                    skillToggles[i].gameObject.SetActive(true);
                    skillToggles[i].GetComponent<Image>().sprite = skills[i].skillImage;
                    var index = i;
                    skillToggles[i].onValueChanged.AddListener((isSelected) =>
                    {
                        if (isSelected)
                        {
                            DisplaySkillInfo(skills[index]);
                        }
                    });
                }
                else
                {
                    skillToggles[i].gameObject.SetActive(false);
                }
            }

            if (skills.Count <= 0) return;
            DisplaySkillInfo(skills[0]);
            skillToggles[0].isOn = true;
        }
    }
}
