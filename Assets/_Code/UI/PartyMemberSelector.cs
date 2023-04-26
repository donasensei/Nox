using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberSelector : MonoBehaviour
{
    [SerializeField] private List<Toggle> characterToggles;
    [SerializeField] private ToggleGroup toggleGroup;

    public void Initialize(BattleManager battleManager, BattleCharacterInfo battleCharacterInfo)
    {
        if (characterToggles == null || characterToggles.Count == 0)
        {
            Debug.LogError("PartyMemberSelector: characterToggles list is not set up properly.");
            return;
        }

        List<CharacterData> playerParty = battleManager.GetPlayerParty();
        if (playerParty == null || playerParty.Count == 0)
        {
            Debug.LogError("BattleManager: playerParty is empty or not set up properly.");
            return;
        }

        UpdateCharacterToggles(battleManager);
        characterToggles[0].isOn = true;
        characterToggles[0].group = toggleGroup;
        battleCharacterInfo.UpdateInfo(playerParty[0]);
        AddToggleListeners(battleManager, battleCharacterInfo);
    }


    void UpdateCharacterToggles(BattleManager battleManager)
    {
        int partySize = battleManager.GetPlayerParty().Count;

        for (int i = 0; i < characterToggles.Count; i++)
        {
            if (i < partySize)
            {
                characterToggles[i].gameObject.SetActive(true);
            }
            else
            {
                characterToggles[i].gameObject.SetActive(false);
            }
        }
    }

    void AddToggleListeners(BattleManager battleManager, BattleCharacterInfo battleCharacterInfo)
    {
        for (int i = 0; i < characterToggles.Count; i++)
        {
            int currentIndex = i;
            characterToggles[i].group = toggleGroup;
            characterToggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    battleCharacterInfo.UpdateInfo(battleManager.GetPlayerParty()[currentIndex]);
                }
            });
        }
    }
}
