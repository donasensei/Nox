using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    // [SerializeField] private Button button;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Text buttonText;

    private CharacterDataWrapper characterDataWrapper;
    private CharacterInfoPanel characterInfoPanel;

    public CharacterDataWrapper CharacterData => characterDataWrapper;

    public void Initialize(CharacterDataWrapper characterDataWrapper, CharacterInfoPanel characterInfoPanel, Color textColor)
    {
        this.characterDataWrapper = characterDataWrapper;
        this.characterInfoPanel = characterInfoPanel;

        buttonText.text = characterDataWrapper.characterName;
        buttonText.color = textColor;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            characterInfoPanel.DisplayCharacterInfo(characterDataWrapper);
        }
    }
}