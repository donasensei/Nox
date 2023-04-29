using System.Collections.Generic;
using _Code.Managers;
using Ink.Runtime;
using UnityEngine;

namespace _Code.Dialogue
{
    [CreateAssetMenu(fileName = "New Ink Data", menuName = "Ink Data")]
    public class InkData : ScriptableObject
    {
        public TextAsset inkFile;
        private Story _story;
        [SerializeField] public List<Sprite> characterImages;
        [SerializeField] public List<Sprite> backgroundImages;

        public Story GetStory()
        {
            if (_story == null)
            {
                _story = new Story(inkFile.text);
            }
            
            _story.BindExternalFunction("GetCharacterName", GetCharacterName);
            return _story;
        }

        private static string GetCharacterName()
        {
            return GameManager.instance.saveData.playerName;
        }
    }
}