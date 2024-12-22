using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static event Action nextClicked;

    [SerializeField] private Button levels;
    [SerializeField] private Button next;
    [SerializeField] private GameObject levelsList;
    [SerializeField] private GameObject levelCompletePopUp;

    private void Start()
    {
        levels.onClick.AddListener(OnLevelsClicked);
        next.onClick.AddListener(OnNextClicked);
    }
    private void OnLevelsClicked()
    {
        levelsList.SetActive(true);
        levelCompletePopUp.SetActive(false);
        SetLevelIndex(0);
    }
    private void OnNextClicked()
    {
        levelCompletePopUp.SetActive(false);
        nextClicked?.Invoke();
    }
    private void SetLevelIndex(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
    }
}
