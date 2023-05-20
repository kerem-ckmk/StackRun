using System;
using UnityEngine;
using Lofelt.NiceVibrations;

public class GameplayController : MonoBehaviour
{
    [Header("References")]
    public LevelManager levelManager;
    public PlayerController playerController;
    public StackManager stackManager;

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
        stackManager.Initialize();
        IsInitialized = true;
    }

    public void PrepareGameplay(int linearLevelIndex)
    {
        levelManager.CreateLevel(linearLevelIndex);
    }

    public void UnloadGameplay()
    {
        levelManager.UnloadLevel();
        playerController.UnloadLevel();
        stackManager.UnloadLevel();
    }

    public void StartGameplay()
    {
        playerController.SetActiveState(true);
        stackManager.SetActiveState(true);
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
            stackManager.StopLastStack();
        }

    }
}
