using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static event Action OnGridUIManagerInitialised;
    public static UIManager Instance { get; private set; }

    [Header("UI references")]
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI objectsCount;
    [SerializeField] TextMeshProUGUI bombsCount;
    [SerializeField] TextMeshProUGUI maxBombsCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: if you want the instance to persist across scenes
    }
    private void OnEnable()
    {
        BombManager.OnBombsCountChanged += UpdateBombsCount;
    }
    private void Start()
    {
        OnGridUIManagerInitialised?.Invoke();
    }
    public void UpdateCount(int count)
    {
        objectsCount.text = count.ToString();
    }
    public void UpdateLevelName(string name) 
    {
        levelName.text = name;
    }
    public void UpdateMaxBombsCount(int count)
    {
        maxBombsCount.text = count.ToString();
    }
    public void UpdateBombsCount(int count)
    {
        bombsCount.text = count.ToString();
    }

}
