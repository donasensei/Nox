using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    public Button LoadButton;

    private void Start()
    {
        if (!GameManager.Instance.CheckSaveFileExist)
        {
            LoadButton.gameObject.SetActive(false);
        }
        else
        {
            LoadButton.gameObject.SetActive(true);
        }
    }
}
