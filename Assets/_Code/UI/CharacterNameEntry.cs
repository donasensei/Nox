using UnityEngine;
using UnityEngine.UI;
// using TMPro;

public class CharacterNameEntry : MonoBehaviour
{
    // [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Text characterNameText;

    public void SetCharacterName(string name)
    {
        characterNameText.text = name;
    }
}
