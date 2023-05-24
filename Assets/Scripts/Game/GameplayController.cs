using System;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [Header("References")]
    public LevelManager levelManager;
    public PlayerController playerController;
    public StackManager stackManager;
    public CameraController cameraController;

    public bool IsInitialized { get; private set; }
    public bool IsActive { get; private set; }

    public int TotalCurrencyReward
    {
        get { return 100 + _currencyReward; }
    }

    public Action<bool> OnGameplayFinished;
    private int _currencyReward;
    public void Initialize()
    {
        levelManager.Initialize();
        levelManager.AddingAmount += LevelManager_AddingAmount;
        playerController.Initialize();
        playerController.WinPlayer += PlayerController_WinPlayer;
        stackManager.Initialize();
        cameraController.Initialize();
        stackManager.OnFailed += StackManager_OnFailed;
        stackManager.NewCenter += StackManager_NewCenter;
        IsInitialized = true;
    }

    public void PrepareGameplay(int linearLevelIndex)
    {
        levelManager.CreateLevel(linearLevelIndex);
        stackManager.Prepare(levelManager.CurrentLevelData.StackCount);
        playerController.Prepare();

        float cameraTargetPositionZ = levelManager.CurrentLevelInstance.finishController.transform.position.z;
        cameraController.Prepare(cameraTargetPositionZ);
    }

    public void UnloadGameplay()
    {
        _currencyReward = 0;
        levelManager.UnloadLevel();
        playerController.UnloadLevel();
        stackManager.UnloadLevel();
    }

    public void StartGameplay()
    {
        stackManager.SetActiveState(true);
        playerController.StartGameplay();

        GameManager.PlaySound(GameConfigs.Instance.ButtonSound);

        IsActive = true;
    }

    private void FinishGameplay(bool success)
    {
        IsActive = false;
        OnGameplayFinished?.Invoke(success);
    }

    private void StackManager_OnFailed()
    {
        playerController.FailedGameplay();
        cameraController.FailedGameplay();
        FinishGameplay(false);
    }

    private void PlayerController_WinPlayer()
    {
        cameraController.FinishGame();
        FinishGameplay(true);
    }

    private void StackManager_NewCenter(Vector3 targetCenter)
    {
        playerController.SetTransformCenter(targetCenter);
    }

    private void LevelManager_AddingAmount(int addingAmount)
    {
        _currencyReward += addingAmount;
    }

    private void Update()
    {
        if (!IsActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            playerController.CheckClick = true;
            stackManager.StopLastStack();
        }

    }
}
