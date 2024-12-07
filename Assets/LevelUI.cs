using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelUI : MonoBehaviour
{
    public static event Action OnLevelSelected;
    [Header("UI references")]
    [SerializeField] GameObject levelName;
    [SerializeField] GameObject levelIcon;
    [SerializeField] GameObject levelButton;

    private void Awake()
    {
        levelButton.GetComponent<Button>().onClick.AddListener(() => GetMyIndex());
    }

    public void SetLevelName(string name)
    {
        levelName.GetComponent<TextMeshProUGUI>().text = name;
    }
    public void SetLevelImage(Sprite image)
    {
        levelIcon.GetComponent<Image>().sprite = image;
    }
    public int GetMyIndex()
    {
        int index = 0;
        index = transform.GetSiblingIndex();
        PlayerPrefs.SetInt("CurrentLevel", index);
        PlayerPrefs.Save();
        Debug.Log($"level index: {index})");
        OnLevelSelected?.Invoke();
        return index;
     
    }
}