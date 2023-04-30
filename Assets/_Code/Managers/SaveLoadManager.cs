using System.Collections.Generic;
using _Code.UI;
using UnityEngine;
using UnityEngine.InputSystem;


namespace _Code.Managers
{
    public class SaveLoadManager : MonoBehaviour
    {
        // UI
        [SerializeField] private List<SaveSlot> saveSlots;

        // Input Actions
        private PlayerInput _playerInput;
        private InputAction _cancelAction;
        
        // Manager
        private GameManager _gameManager;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            _cancelAction = _playerInput.actions["Cancel"];
        }

        private void Start()
        {
            _gameManager = GameManager.instance;
            UpdateSlots(_gameManager.state);
            
            Hide();
        }

        #region Save Slot Action

        private void UpdateSlots(GameState state)
        {
            for (var idx = 0; idx < saveSlots.Count; idx++)
            {
                var slot = saveSlots[idx];
                SaveData saveData;
                if (_gameManager.IsSaveFileExist(idx))
                {
                    saveData = _gameManager.LoadData(idx);
                }
                else
                {
                    saveData = new SaveData
                    {
                        playerName = "Empty Slot",
                        currentDay = 0,
                        currentLocation = "N/A"
                    };
                }
                AddListeners(slot, state, idx);
                UpdateSlotUI(saveData, slot, idx);
            }
        }
        
        private void AddListeners(SaveSlot slot, GameState state, int idx)
        {
            slot.button.onClick.RemoveAllListeners();
            switch (state)
            {
                case GameState.Save:
                    slot.button.onClick.AddListener(() => _gameManager.SaveGame(idx));
                    break;
                case GameState.Load:
                    if (_gameManager.IsSaveFileExist(idx))
                    {
                        slot.button.onClick.AddListener(() => _gameManager.LoadGame(idx));
                    }
                    break;
            }
        }

        private static void UpdateSlotUI(SaveData data, SaveSlot slot, int idx)
        {
            var i = idx + 1;
            slot.slotNumberText.text = i.ToString();
            slot.slotDayText.text = "Day\n" + data.currentDay;
            slot.slotPlayerNameText.text = data.playerName;
            slot.slotLocationText.text = data.currentLocation;
        }
        
        #endregion
        
        #region 세이브 슬롯 UI

        public void Show()
        {
            UpdateSlots(_gameManager.state);
            this.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        #endregion

        #region Input Actions

        private void OnEnable()
        {
            _cancelAction.performed += OnCancelPerformed;
        }
        
        private void OnDisable()
        {
            _cancelAction.performed -= OnCancelPerformed;
        }
        
        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            Hide();
        }
        
        #endregion
    }
}
