using System;
using System.Collections.Generic;
using _Code.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Code.Managers
{
    public class SaveLoadManager : MonoBehaviour
    {
        // UI Prefabs
        /*[SerializeField] private NameInputPanel nameInputPanel;
        [SerializeField] private BoolDialog boolDialog;
        [SerializeField] private ErrorDialog errorDialog;*/
        
        // UI
        [SerializeField] private List<SaveSlot> saveSlots;
    
        // Manager
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.instance;
            UpdateSlots(_gameManager.state);
        }

        #region 세이브 슬롯 액션

        private void UpdateSlots(GameState state)
        {
            // ReSharper disable once PossibleNullReferenceException
            for (var idx = 0; idx < saveSlots.Count; idx++)
            {
                var slot = saveSlots[idx];
                // ReSharper disable once PossibleNullReferenceException
                var saveData = _gameManager.LoadData(idx);
                AddListeners(slot, state, idx);
                UpdateSlotUI(saveData, slot, idx);
            }
        }
        
        private void AddListeners(SaveSlot slot, GameState state, int idx)
        {
            slot.button.onClick.RemoveAllListeners();
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (state)
            {
                case GameState.Save:
                    // ReSharper disable once AccessToModifiedClosure
                    slot.button.onClick.AddListener(() => _gameManager.SaveGame(idx));
                    break;
                case GameState.Load:
                    // ReSharper disable once AccessToModifiedClosure
                    slot.button.onClick.AddListener(() => _gameManager.LoadGame(idx));
                    break;
                /*case GameState.NewGame:
                    // ReSharper disable once AccessToModifiedClosure
                    slot.button.onClick.AddListener(() => _gameManager.NewGame(idx));
                    break;*/
                default:
                    break;
            }
        }

        private static void UpdateSlotUI(SaveData data, SaveSlot slot, int idx)
        {
            slot.slotNumberText.text = idx.ToString();
            slot.slotDayText.text = data.currentDay.ToString();
            slot.slotPlayerNameText.text = data.playerName;
            slot.slotLocationText.text = data.currentLocation;
        }
        #endregion
    }
}
