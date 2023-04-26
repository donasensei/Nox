using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum TurnState
    {
        PlayerPartyActionSelect,
        EnemyPartyActionSelect,
        ExecuteAction,
        CheckCharacterState,
        CheckWinOrLose
    }
    public TurnState currentTurnState;

    // Data
    [SerializeField] private List<CharacterData> playerParty;
    [SerializeField] private List<CharacterData> enemyParty;

    // UI
    [SerializeField] private BattleCharacterInfo infoUI;
    [SerializeField] private PartyMemberSelector partyMemberSelector;

    void Start()
    {
        // Get player characters from GameManager
        playerParty = GameManager.Instance.ConvertToCharacterDataList(GameManager.Instance.SaveData.partyList);

        // Initialize turn state
        currentTurnState = TurnState.PlayerPartyActionSelect;

        // Initialize PartyMemberSelector
        partyMemberSelector.Initialize(this, infoUI);
    }

    void Update()
    {
        // Manage turn states here
        switch (currentTurnState)
        {
            case TurnState.PlayerPartyActionSelect:
                // Handle player action selection
                break;

            case TurnState.EnemyPartyActionSelect:
                // Handle enemy action selection
                break;

            case TurnState.ExecuteAction:
                // Execute actions
                break;

            case TurnState.CheckCharacterState:
                // Check character states
                break;

            case TurnState.CheckWinOrLose:
                // Check win or lose conditions
                break;
        }
    }

    public List<CharacterData> GetPlayerParty()
    {
        return playerParty;
    }
}
