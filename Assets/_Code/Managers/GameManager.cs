using System.Collections.Generic;
using System.Linq;
using _Code.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.Managers
{
    public class GameManager : MonoBehaviour
    {
        // State
        public GameState state;

        // SaveData
        private readonly string[] _saveFileNames = { "sav01", "sav02", "sav03", "sav04" };
        private const string Key = "SaveData";
        public SaveData saveData;
        
        // Frame Rate
        [SerializeField] private int frame = 60;

        // Singleton
        public static GameManager instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Limit the frame rate
            Application.targetFrameRate = frame;

            // UpdateReferences();
        }


        private void Start()
        { 
            // Set State
            state = GameState.MainMenu;
        }

        #region Scene관련

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            saveData.lastSceneIndex = scene.buildIndex;
            saveData.lastSceneName = scene.name;
            // UpdateReferences();
        }
        
        #endregion
        
        public void ChangeDay() 
        {
            if (saveData.dayNight == DayNight.Night)
            {
                saveData.dayNight = DayNight.Day;
                saveData.currentDay++;
            }
            else
            {
                saveData.dayNight = DayNight.Night;
            }
        }

        #region 캐릭터 관련

        public void UpdateCharacterData(CharacterDataWrapper updatedCharacterData)
        {
            for (var i = 0; i < saveData.partyList.Count; i++)
            {
                if (saveData.partyList[i].characterName != updatedCharacterData.characterName) continue;
                saveData.partyList[i] = updatedCharacterData;
                return;
            }

            for (var i = 0; i < saveData.characterList.Count; i++)
            {
                if (saveData.characterList[i].characterName != updatedCharacterData.characterName) continue;
                saveData.characterList[i] = updatedCharacterData;
                return;
            }
        }

        public List<CharacterDataWrapper> ConvertToWrapperList(IEnumerable<CharacterData> characterDataList)
        {
            return characterDataList.Select(characterData => new CharacterDataWrapper(characterData)).ToList();
        }

        public static List<CharacterData> ConvertToCharacterDataList(List<CharacterDataWrapper> wrapperList)
        {
            List<CharacterData> characterDataList = new();

            foreach (var wrapper in wrapperList)
            {
                var characterData = ScriptableObject.CreateInstance<CharacterData>();
                characterData.characterName = wrapper.characterName;
                characterData.characterDesc = wrapper.characterDesc;
                characterData.characterImage = wrapper.characterImage;
                characterData.characterStat = wrapper.characterStat;
                characterData.currentHealth = wrapper.currentHealth;
                characterData.currentMana = wrapper.currentMana;
                characterData.level = wrapper.level;
                characterData.experience = wrapper.experience;
                characterData.characterSkill = wrapper.characterSkill;
                characterData.defaultSkill = wrapper.defaultSkill;

                characterDataList.Add(characterData);
            }

            return characterDataList;
        }

        private static CharacterDataWrapper ConvertToWrapper(CharacterData characterData)
        {
            CharacterDataWrapper wrapper = new(characterData);
            return wrapper;
        }

        #endregion

        #region Save관련

        public void SaveGame(int index)
        {
            ES3.Save(Key, saveData, _saveFileNames[index]);
        }

        public SaveData LoadData(int index)
        {
            return ES3.Load(Key, _saveFileNames[index], saveData);
        }

        public void LoadGame(int index)
        {
            saveData = LoadData(index);
            CustomSceneManager.instance.LoadScene(saveData.lastSceneIndex);
        }

        public bool IsSaveFileExist()
        {
            return _saveFileNames.Any(ES3.FileExists);
        }
        
        public bool IsSaveFileExist(int index)
        {
            return ES3.FileExists(_saveFileNames[index]);
        }
        
        public void NewGame()
        {
            saveData = NewSaveData("테스트"); //TODO: 이름 입력 받을 것
            SaveGame(0);
            CustomSceneManager.instance.LoadScene("SkillTest"); //TODO: 나중에 변경할 것
        }
        public void DeleteData(int index)
        {
            ES3.DeleteFile(_saveFileNames[index]);
        }

        private static SaveData NewSaveData(string playerName)
        {
            var data = new SaveData
            {
                playerName = playerName,
                currentLocation = "엘더스 지하감옥",
                currentDay = 1,
                dayNight = DayNight.Night,
                lastSceneIndex = 0,
                lastSceneName = "Intro01",
                stones = 0,
                characterList = new List<CharacterDataWrapper>(),
                partyList = new List<CharacterDataWrapper>(),
                stageDataList = new List<StageData>(),
                flags = new Flags()
            };
            
            // 캐릭터 추가
            var player = Resources.Load("Characters/Player") as CharacterData;
            var friend = Resources.Load("Characters/Friend") as CharacterData;
            var rider = Resources.Load("Characters/Rider") as CharacterData;

            data.partyList.Add(ConvertToWrapper(player));
            data.partyList.Add(ConvertToWrapper(friend));
            data.partyList.Add(ConvertToWrapper(rider));

            foreach (var character in data.partyList)
            {
                character.UpdateStats();
                character.SetCurrent();
            }
            return data;
        }
    
        #endregion
    }

    [System.Serializable]
    public enum DayNight
    {
        Day,
        Night
    }

    [System.Serializable]
    public class SaveData
    {
        // SaveFile Data
        public string playerName;
        public string currentLocation;
        public uint currentDay;

        // Day/Night
        public DayNight dayNight;

        // Last Scene
        public int lastSceneIndex;
        public string lastSceneName;

        // Currency
        public uint stones;

        // Lists 
        // Character list
        public List<CharacterDataWrapper> characterList;
        // Party list
        public List<CharacterDataWrapper> partyList;
        // Stage Data List
        public List<StageData> stageDataList;

        // Flags
        public Flags flags;
    }

    [System.Serializable]
    public struct Flags
    {
        // Add Story or Quest Flags Here ↓

    }

    public enum GameState
    {
        MainMenu,
        Save,
        Load
    }
}