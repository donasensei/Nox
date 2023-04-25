using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Ink Data", menuName = "Ink Data")]
public class InkData : ScriptableObject
{
    public TextAsset inkFile;
    private Story story;
    public List<Sprite> characterImages;

    public Story GetStory()
    {
        if (story == null)
        {
            story = new Story(inkFile.text);
        }

        return story;
    }
}