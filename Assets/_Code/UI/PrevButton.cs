using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrevButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        CustomSceneManager.Instance.LoadPreviousScene();
    }
}
