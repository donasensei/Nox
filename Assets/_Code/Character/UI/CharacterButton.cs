using _Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Character.UI
{
    public class CharacterButton : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text buttonText;

        private CharacterInfoPanel _characterInfoPanel;

        public CharacterDataWrapper dataWrapper { get; private set; }

        public void Initialize(CharacterDataWrapper characterDataWrapper, CharacterInfoPanel characterInfoPanel, Color textColor)
        {
            this.dataWrapper = characterDataWrapper;
            this._characterInfoPanel = characterInfoPanel;

            buttonText.text = characterDataWrapper.characterName;
            buttonText.color = textColor;
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            if (isOn)
            {
                _characterInfoPanel.DisplayCharacterInfo(dataWrapper);
            }
        }
    }
}