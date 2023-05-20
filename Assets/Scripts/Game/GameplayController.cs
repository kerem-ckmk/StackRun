﻿using System;
using UnityEngine;
using Lofelt.NiceVibrations;

public class GameplayController : MonoBehaviour
{
    [Header("References")]
    public LevelManager levelManager; 
    public PlayerController playerController;

    public bool IsInitialized { get; private set; }
    public bool IsActive { get; private set; }

    public int TotalCurrencyReward
    {
        get { return 100; }
    }

    public Action<bool> OnGameplayFinished;

    public void Initialize()
    {
        levelManager.Initialize();
        playerController.Initialize();

        IsInitialized = true;
    }

    public void PrepareGameplay(int linearLevelIndex)
    {
        levelManager.CreateLevel(linearLevelIndex);
    }

    public void UnloadGameplay()
    {
        levelManager.UnloadLevel();
    }

    public void StartGameplay()
    {
        IsActive = true;
    }

    private void FinishGameplay(bool success)
    {
        IsActive = false;

        OnGameplayFinished?.Invoke(success);
    }

    private void Update()
    {
        if (!IsActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            FinishGameplay(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FinishGameplay(false);
        }
    }
}
