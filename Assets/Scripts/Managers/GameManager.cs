﻿using System;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance; 

    [Header("UI")] 
    [SerializeField] private Start startMenu;
    [SerializeField] private GameOver gameOverMenu;
    [SerializeField] private LevelWin levelWinMenu;
    
    [Header("Controllers")]
    [SerializeField] private HelixController helixController;
    [SerializeField] private BallController ballController;

    [Header("Debug")] 
    [SerializeField] private int DebugLevelNo = 0;
    
    private int currLevel = 0;
    
    public int PassedRingCnt { get; private set; }
    
    public int HighScore { get; private set; }
    public int Score { get; private set; }

    public Action OnScoreChange;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log($"{Instance.name} {gameObject.name} An instance of this singleton already exists");
            // throw new System.Exception($"{Instance.name} {gameObject.name} An instance of this singleton already exists.");
        }
        else
        {
            Instance = this;
        }

        if (startMenu == null)
            startMenu = FindObjectOfType<Start>();
        
        if (gameOverMenu == null)
            gameOverMenu = FindObjectOfType<GameOver>();
        
        if (levelWinMenu == null)
            levelWinMenu = FindObjectOfType<LevelWin>();
        
        if (helixController == null)
            helixController = FindObjectOfType<HelixController>();

        if (ballController == null)
            ballController = FindObjectOfType<BallController>();

        startMenu.OnStart += OnGameStart;

        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        currLevel = PlayerPrefs.GetInt("CurrLevel", 0);
    }

    void OnDestroy()
    {
        startMenu.OnStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        LoadLevel();
    }
    
    public void LevelWin()
    {
        levelWinMenu.OnLevelWin();
    }

    public void GoToNextLevel()
    {
        currLevel++;
        
        // Clamping the level-no for safety in case it increase beyond the max level
        currLevel = Mathf.Clamp(currLevel, 0, helixController.levelData.levels.Count - 1);
        PlayerPrefs.SetInt("CurrLevel", currLevel);
        
        LoadLevel();
    }

    private void LoadLevel()
    {
        Score = 0;
        PassedRingCnt = 0;
        ballController.ResetBall();
        helixController.LoadLevel(currLevel);
        OnScoreChange?.Invoke();
    }

    public void GameOver()
    {
        gameOverMenu.OnGameOver();
    }

    public void RestartLevel()
    {
        LoadLevel();
    }
    
    public void AddScore(int scoreToAdd)
    {
        Score += scoreToAdd;

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", Score);
        }
        
        OnScoreChange?.Invoke();
    }

    public void IncreasePassedRingCnt()
    {
        PassedRingCnt++;
    }

    [ContextMenu("Debug Level")]
    private void DebugLevel()
    {
        currLevel = DebugLevelNo;
        LoadLevel();
    }
}