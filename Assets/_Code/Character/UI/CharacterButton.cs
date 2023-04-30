using _Code.Character;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.UI
{
    public class CharacterButton : MonoBehaviour
    {
        // [SerializeField] private Button button;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text buttonText;

        private CharacterInfoPanel _characterInfoPanel;

        public CharacterDataWrapper characterData { get; private set; }

        public void Initialize(CharacterDataWrapper characterDataWrapper, CharacterInfoPanel characterInfoPanel, Color textColor)
        {
            this.characterData = characterDataWrapper;
            this._characterInfoPanel = characterInfoPanel;

            buttonText.text = characterDataWrapper.characterName;
            buttonText.color = textColor;
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                _characterInfoPanel.DisplayCharacterInfo(characterData);
            }
        }
    }
}