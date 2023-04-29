using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class BattleSkillSelector : MonoBehaviour
    {
        [SerializeField] private List<Image> skillIcons = new();
        [SerializeField] private Text skillName;
        [SerializeField] private Text skillDescription;
        [SerializeField] private Text skillCost;
        [SerializeField] private Text skillType;


    }
}
