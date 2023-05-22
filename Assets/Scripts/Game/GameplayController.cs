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
        stackManager.OnFailed += StackManager_OnFailed;
        stackManager.NewCenter += StackManager_NewCenter;
        IsInitialized = true;
    }

    private void StackManager_NewCenter(Vector3 targetCenter)
    {
        playerController.SetTransformCenter(targetCenter);
    }

    public void PrepareGameplay(int linearLevelIndex)
    {
        levelManager.CreateLevel(linearLevelIndex);
        playerController.Prepare();
    }

    public void UnloadGameplay()
    {
        levelManager.UnloadLevel();
        playerController.UnloadLevel();
        stackManager.UnloadLevel();
    }

    public void StartGameplay()
    {
        stackManager.SetActiveState(true);
        playerController.StartGameplay();
        IsActive = true;
    }

    private void FinishGameplay(bool success)
    {
        IsActive = false;
        playerController.SetActiveState(false);
        OnGameplayFinished?.Invoke(success);
    }

    private void StackManager_OnFailed()
    {
        playerController.FailedGameplay();
        FinishGameplay(false);
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
