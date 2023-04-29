using System.Collections;
using System.Collections.Generic;
using _Code.Character;
using _Code.UI;
using UnityEngine;
using UnityEngine.UI;

namespace _Code.Managers
{
    public class CharacterMenuManager : MonoBehaviour
    {
        // Managers 
        private GameManager _gameManager;
        
        // UI
        [SerializeField] private CharacterButton characterButtonPrefab;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private CharacterInfoPanel characterInfoPanel;
        [SerializeField] private ToggleGroup characterToggleGroup;

        // Colors
        [Header("Text Colors")]
        [SerializeField] private Color playerPartyColor = Color.black;
        [SerializeField] private Color characterListColor = Color.gray;

        private readonly List<CharacterButton> _characterButtons = new();

        private void Start()
        {
            PopulateCharacterList();
            StartCoroutine(DisplayFirstCharacterInfo());
        }

        private void PopulateCharacterList()
        {
            // Load and convert party list and character list
            var partyList = _gameManager.saveData.partyList;
            var characterList = _gameManager.saveData.characterList;

            // Display characters in the party list
            foreach (var character in partyList)
            {
                CreateCharacterButton(character, playerPartyColor);
            }

            // Display characters in the character list
            foreach (var character in characterList)
            {
                CreateCharacterButton(character, characterListColor);
            }
        }

        private void CreateCharacterButton(CharacterDataWrapper character, Color textColor)
        {
            var button = Instantiate(characterButtonPrefab, contentTransform);
            button.Initialize(character, characterInfoPanel, textColor);
            button.GetComponent<Toggle>().group = characterToggleGroup;
            _characterButtons.Add(button);
        }

        private IEnumerator DisplayFirstCharacterInfo()
        {
            yield return new WaitForEndOfFrame();

            if (_characterButtons.Count > 0)
            {
                characterInfoPanel.DisplayCharacterInfo(_characterButtons[0].CharacterData);
            }
        }
    }
}
