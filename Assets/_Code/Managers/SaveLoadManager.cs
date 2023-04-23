using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // UI
    public NameInputPanel nameInputPanel;
    public BoolDialog boolDialog;
    public ErrorDialog errorDialog;

    // Save
    public List<SaveSlot> saveSlots;
    private readonly string[] saveFileNames = { "sav01", "sav02", "sav03", "sav04" };
    private int currentSaveSlotIndex;
    
    // Error Messages
    private const string NoSaveFileError = "저장된 데이터가 없습니다.";
    private const string SaveFileDelected = "저장된 데이터가 삭제되었습니다.";
    private const string SaveFileOverwritten = "이미 저장된 파일이 있습니다. 덮어쓰시겠습니까?";


    private void Start()
    {
        UpdateSaveSlots();
        nameInputPanel.confirmButton.onClick.AddListener(ConfirmPlayerName);
        nameInputPanel.Hide();
    }

    public void SlotSelected(int saveFileIndex)
    {
        currentSaveSlotIndex = saveFileIndex;

        if (GameManager.Instance.state == GameManager.GameState.NewGame)
        {
            if (!ES3.FileExists(saveFileNames[saveFileIndex]))
            {
                SetCharacterName(saveFileIndex);
            }
            else
            {
                boolDialog.SetInfoText("이미 저장된 파일이 있습니다. 덮어쓰시겠습니까?");
                boolDialog.Show();
            }
        }
        else if (GameManager.Instance.state == GameManager.GameState.LoadGame)
        {
            LoadGame(saveFileIndex);
        }
    }

    public void NewSaveInit()
    {
        DeleteSave();
        errorDialog.OnDialogHidden += OnDialogHiddenHandler;
    }

    private void OnDialogHiddenHandler()
    {
        SetCharacterName(currentSaveSlotIndex);
        errorDialog.OnDialogHidden -= OnDialogHiddenHandler;
    }

    public void DeleteSave()
    {
        ES3.DeleteFile(saveFileNames[currentSaveSlotIndex]);
        errorDialog.SetErrorText(SaveFileDelected);
        errorDialog.Show();
    }

    private void SetCharacterName(int saveFileIndex)
    {
        currentSaveSlotIndex = saveFileIndex;
        nameInputPanel.Show();
    }

    public SaveData Load(int saveFileIndex)
    {
        string saveFileName = saveFileNames[saveFileIndex];
        if (ES3.FileExists(saveFileName))
        {
            SaveData loadedData = ES3.Load<SaveData>("saveData", saveFileName);
            UpdateSlotUI(saveFileIndex, loadedData);
            return loadedData;
        }
        return null;
    }

    private void UpdateSlotUI(int saveFileIndex, SaveData loadedData)
    {
        saveSlots[saveFileIndex].slotNumberText.text = (saveFileIndex + 1).ToString();
        saveSlots[saveFileIndex].slotPlayerNameText.text = loadedData.playerName;
        saveSlots[saveFileIndex].slotLocationText.text = loadedData.currentLocation;
        saveSlots[saveFileIndex].slotDayText.text = "Day\n" + loadedData.currentDay.ToString();
    }

    public void LoadGame(int saveFileIndex)
    {
        string saveFileName = saveFileNames[saveFileIndex];
        if (ES3.FileExists(saveFileNames[saveFileIndex]))
        {
            GameManager.Instance.SaveData = ES3.Load<SaveData>("saveData", saveFileName);
            Debug.Log("Load SaveData : " + saveFileName);
            SceneManager.Instance.LoadScene(GameManager.Instance.SaveData.lastSceneIndex);
        }
        else
        {
            Debug.LogWarning("No Savefile in " + saveFileNames[saveFileIndex].ToString());
            errorDialog.SetErrorText(NoSaveFileError.ToString());
            errorDialog.Show();
        }
    }

    private void UpdateSaveSlots()
    {
        for (int i = 0; i < saveFileNames.Length; i++)
        {
            if (ES3.FileExists(saveFileNames[i]))
            {
                Load(i);
            }
            else
            {
                Debug.LogWarning("No Savefile in " + saveFileNames[i].ToString());
            }
        }
    }

    public void ConfirmPlayerName()
    {
        InitNewGameData();
    }

    private void InitNewGameData()
    {
        GameManager gameManager = GameManager.Instance;
        string playerName = nameInputPanel.playerNameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            string saveFileName = saveFileNames[currentSaveSlotIndex];
            InitialSaveData(playerName);
            ES3.Save<SaveData>("saveData", gameManager.SaveData, saveFileName);
            UpdateSaveSlots();
            nameInputPanel.Hide();
        }

        SceneManager.Instance.LoadScene("Daytime"); //TODO: 나중에 Tutorial로 바꾸기.
    }

    private void InitialSaveData(string name)
    {
        GameManager gameManager = GameManager.Instance;
        SaveData saveData = new()
        {
            playerName = name,
            currentLocation = "엘더스 지하감옥",
            currentDay = 1,
            dayNight = DayNight.Night,
            lastSceneIndex = 2, // TODO: 나중에 Tutorial로 바꾸기.
            stones = 0,

            // Lists
            partyList = new List<CharacterDataWrapper>(),
            characterList = new List<CharacterDataWrapper>(),
            stageDataList = new List<StageData>()
        };

        gameManager.SaveData = saveData;
        CharacterData mainCharacter = MakeNewCharacter(name);
        // DebugCharacterInfo(mainCharacter);

        gameManager.SaveData.partyList.Add(gameManager.ConvertToWrapper(mainCharacter));
    }

    private static CharacterData MakeNewCharacter(string name)
    {
        CharacterData character = ScriptableObject.CreateInstance<CharacterData>();
        character.characterName = name;
        character.characterDesc = "플레이어: " + name;
        character.characterImage = null;
        character.characterStat = new CharacterStat
        {
            Strength = 3,
            Magic = 3,
            Vitality = 3,
            Blessing = 3,
        };

        Calc(character);
        return character;
    }

    private static void Calc(CharacterData mainCharacter)
    {
        // Calculate Max Health and Max Mana
        mainCharacter.UpdateMaxHealthAndMana();
        mainCharacter.currentMana = mainCharacter.characterStat.MaxMana;
        mainCharacter.currentHealth = mainCharacter.characterStat.MaxHealth;
        mainCharacter.level = 1;
        mainCharacter.experience = 0;
        mainCharacter.characterSkill = AddBasicSkillSet();
    }

    private static List<SkillData> AddBasicSkillSet()
    {
        return new List<SkillData>
        {
            // Add Basic Skills : DEBUG TEST SKILL
            Resources.Load<SkillData>("Skills/Slash"),
            Resources.Load<SkillData>("Skills/Dual Slash"),
            Resources.Load<SkillData>("Skills/Triple Slash")
        };
    }

    private static void DebugCharacterInfo(CharacterData mainCharacter)
    {
        Debug.Log("Main Character: " + mainCharacter.characterName);
        Debug.Log("Main Character: " + mainCharacter.characterDesc);
        Debug.Log("Main Character: " + mainCharacter.characterImage);
        Debug.Log("Main Character: " + mainCharacter.characterStat.Strength);
        Debug.Log("Main Character: " + mainCharacter.characterStat.Magic);
        Debug.Log("Main Character: " + mainCharacter.characterStat.Vitality);
        Debug.Log("Main Character: " + mainCharacter.characterStat.Blessing);
        Debug.Log("Main Character: " + mainCharacter.currentMana);
        Debug.Log("Main Character: " + mainCharacter.currentHealth);
        Debug.Log("Main Character: " + mainCharacter.level);
        Debug.Log("Main Character: " + mainCharacter.experience);
        Debug.Log("Main Character: " + mainCharacter.characterSkill[0].skillName);
        Debug.Log("Main Character: " + mainCharacter.characterSkill[1].skillName);
        Debug.Log("Main Character: " + mainCharacter.characterSkill[2].skillName);
    }
}
