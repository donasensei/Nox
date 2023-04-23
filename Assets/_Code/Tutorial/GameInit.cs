using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameInit : MonoBehaviour
{
    public enum Tutorial01State
    {
        SetName,
        End
    }
    [SerializeField] public Tutorial01State State;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject nameMenu;

    [SerializeField] private Button endButton;

    public void Awake()
    {
        State = Tutorial01State.SetName;
    }

    private void Update()
    {
        switch (State)
        {
            case Tutorial01State.SetName:
                // 이름 입력창 활성화
                nameMenu.SetActive(true);

                // InputField가 비었다면 확인 버튼 비활성화
                if (inputName.text == "")
                {
                    confirmButton.interactable = false;
                }
                else
                {
                    confirmButton.interactable = true;
                }
                break;
            case Tutorial01State.End:
                // 이름 입력창 비활성화
                nameMenu.SetActive(false);
                // End 버튼 활성화
                endButton.interactable = true;
                break;
        }
    }

    public void SetPlayerName()
    {
        // Input Field 의 내용으로 Player 이름 설정
        if (inputName != null && State == Tutorial01State.SetName)
        {
            // PartyList가 비어있는지 확인
            if (GameManager.Instance.SaveData.partyList[0] != null)
            {
                GameManager.Instance.SaveData.partyList[0].characterName = inputName.text;
                State = Tutorial01State.End;
            }
            else
            {
                Debug.LogError("파티가 비었습니다!");
            }
        }
    }
}
