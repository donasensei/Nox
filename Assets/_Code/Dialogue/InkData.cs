using System.Collections.Generic;
using _Code.Managers;
using Ink.Runtime;
using UnityEngine;

namespace _Code.Dialogue
{
    [CreateAssetMenu(fileName = "New Ink Data", menuName = "Ink Data")]
    public class InkData : ScriptableObject
    {
        // Ink Data
        public TextAsset inkFile;
        private Story _story;
        
        private static readonly HashSet<Story> StoriesWithBoundFunctions = new HashSet<Story>();
        
        // Images
        [SerializeField] public List<Sprite> characterImages;
        [SerializeField] public List<Sprite> backgroundImages;

        public Story GetStory()
        {
            if (_story == null)
            {
                _story = new Story(inkFile.text);
            }

            if (StoriesWithBoundFunctions.Contains(_story)) return _story;
            _story.BindExternalFunction("getCharacterName", GetCharacterName);
            StoriesWithBoundFunctions.Add(_story);

            return _story;
        }

        private static string GetCharacterName()
        {
            return GameManager.instance.saveData.playerName;
        }
    }
}