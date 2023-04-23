using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuManager : MonoBehaviour
{
    // UI
    [SerializeField] private CharacterButton characterButtonPrefab;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private CharacterInfoPanel characterInfoPanel;
    [SerializeField] private ToggleGroup characterToggleGroup;

    [Header("Text Colors")]
    [SerializeField] private Color playerPartyColor = Color.black;
    [SerializeField] private Color characterListColor = Color.gray;

    private readonly List<CharacterButton> characterButtons = new();

    private void Start()
    {
        PopulateCharacterList();
        StartCoroutine(DisplayFirstCharacterInfo());
    }

    private void PopulateCharacterList()
    {
        // Load and convert party list and character list
        List<CharacterDataWrapper> partyList = GameManager.Instance.SaveData.partyList;
        List<CharacterDataWrapper> characterList = GameManager.Instance.SaveData.characterList;

        // Display characters in the party list
        foreach (CharacterDataWrapper character in partyList)
        {
            CreateCharacterButton(character, playerPartyColor);
        }

        // Display characters in the character list
        foreach (CharacterDataWrapper character in characterList)
        {
            CreateCharacterButton(character, characterListColor);
        }
    }

    private void CreateCharacterButton(CharacterDataWrapper character, Color textColor)
    {
        CharacterButton button = Instantiate(characterButtonPrefab, contentTransform);
        button.Initialize(character, characterInfoPanel, textColor);
        button.GetComponent<Toggle>().group = characterToggleGroup;
        characterButtons.Add(button);
    }

    private IEnumerator DisplayFirstCharacterInfo()
    {
        yield return new WaitForEndOfFrame();

        if (characterButtons.Count > 0)
        {
            characterInfoPanel.DisplayCharacterInfo(characterButtons[0].CharacterData);
        }
    }
}
