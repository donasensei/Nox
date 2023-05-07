using System;
using _Code.Character;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Battle.UI
{
    public class BattleCharacterButton : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text buttonText;

        private BattleCharacterInfo _characterInfoPanel;
        
        public event Action<CharacterData> OnCharacterSelected;
        public CharacterData characterData { get; private set; }

        public void Initialize(CharacterData data, BattleCharacterInfo characterInfoPanel)
        {
            this.characterData = data;
            this._characterInfoPanel = characterInfoPanel;

            buttonText.text = characterData.characterName;
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (!isOn) return;
            _characterInfoPanel.UpdateInfo(characterData);
            OnCharacterSelected?.Invoke(characterData);
        }
    }
}
