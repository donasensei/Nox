using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Ink Data", menuName = "Ink Data")]
public class InkData : ScriptableObject
{
    public TextAsset inkFile;
    private Story story;
    public List<Sprite> characterImages;
    public List<Sprite> backgroundImages;

    private static HashSet<Story> storiesWithBoundFunctions = new HashSet<Story>();

    public Story GetStory()
    {
        if (story == null)
        {
            story = new Story(inkFile.text);
        }

        if (!storiesWithBoundFunctions.Contains(story))
        {
            story.BindExternalFunction("getCharacterName", () => GetCharacterName());
            storiesWithBoundFunctions.Add(story);
        }

        return story;
    }

    private string GetCharacterName()
    {
        return GameManager.Instance.SaveData.playerName;
    }
}