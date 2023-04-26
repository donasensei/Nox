using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // State
    public enum GameState
    {
        MainMenu,
        NewGame,
        LoadGame,
        SaveGame,
        DayTime,
        Explore
    }
    public GameState state;

    // Data
    private readonly string[] saveFileNames = { "sav01", "sav02", "sav03", "sav04" };

    [SerializeField] private SaveData saveData;
    public SaveData SaveData { get { return saveData; } set { saveData = value; } }

    // Frame Rate
    [SerializeField] private int frame = 60;

    // Singleton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Limit the frame rate
        Application.targetFrameRate = frame;
    }

    private void Start()
    { 
        // Set State
        state = GameState.MainMenu;

        saveData = new SaveData
        {
            characterList = new List<CharacterDataWrapper>(),
            partyList = new List<CharacterDataWrapper>(),
            stageDataList = new List<StageData>()
        };
    }


    public DayNight GetDayNight() 
    {
        DayNight daynight = SaveData.dayNight;
        return daynight;
    }

    public void UpdateCharacterData(CharacterDataWrapper updatedCharacterData)
    {
        for (int i = 0; i < SaveData.partyList.Count; i++)
        {
            if (SaveData.partyList[i].characterName == updatedCharacterData.characterName)
            {
                SaveData.partyList[i] = updatedCharacterData;
                return;
            }
        }

        for (int i = 0; i < SaveData.characterList.Count; i++)
        {
            if (SaveData.characterList[i].characterName == updatedCharacterData.characterName)
            {
                SaveData.characterList[i] = updatedCharacterData;
                return;
            }
        }
    }

    public List<CharacterDataWrapper> ConvertToWrapperList(List<CharacterData> characterDataList)
    {
        List<CharacterDataWrapper> wrapperList = new();

        foreach (CharacterData characterData in characterDataList)
        {
            CharacterDataWrapper wrapper = new(characterData);
            wrapperList.Add(wrapper);
        }

        return wrapperList;
    }

    public List<CharacterData> ConvertToCharacterDataList(List<CharacterDataWrapper> wrapperList)
    {
        List<CharacterData> characterDataList = new();

        foreach (CharacterDataWrapper wrapper in wrapperList)
        {
            CharacterData characterData = ScriptableObject.CreateInstance<CharacterData>();
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

    public CharacterDataWrapper ConvertToWrapper(CharacterData characterData)
    {
        CharacterDataWrapper wrapper = new(characterData);
        return wrapper;
    }

    public bool CheckSaveFileExist
    {
        get
        {
            if (!ES3.FileExists(saveFileNames[0]) && !ES3.FileExists(saveFileNames[1]) && !ES3.FileExists(saveFileNames[2]) && !ES3.FileExists(saveFileNames[3]))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
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
    public int saveFileIndex;

    public string playerName;
    public string currentLocation;
    public uint currentDay;

    // Day/Night
    public DayNight dayNight;

    // Last Scene
    public int lastSceneIndex;

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
    public bool isTutorialStoryDone;
    public bool isTutorialBattleDone;
    public bool isTutorialSkillDone;
    public bool isTutorialExploreBattleDone;
    public bool isTutorialExploreResourceDone;
    public bool isTutorialExploreDone;
    public bool isTutorialDone;

    public void Init()
    {
        isTutorialStoryDone = false;
        isTutorialBattleDone = false;
        isTutorialSkillDone = false;
        isTutorialExploreBattleDone = false;
        isTutorialExploreResourceDone = false;
        isTutorialExploreDone = false;
        isTutorialDone = false;
    }
}