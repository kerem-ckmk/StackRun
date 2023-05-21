using System;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [Header("References")]
    public Material[] stackMaterials;
    public StackController stackControllerPrefab;
    public List<StackController> StackControllers { get; private set; }
    public bool IsInitialized { get; private set; }
    public bool IsActive { get; private set; }
    public StackController ActiveStackController { get; private set; }

    public event Action OnFailed;

    private int _materialIndex = -1;
    private PositionStatus _lastPosition;
    private StackController _previousStackController;
    

    public void Initialize()
    {
        StackControllers = new List<StackController>();
        StackControllers.Clear();
        _previousStackController = null;

        IsInitialized = true;
    }

    public void SetActiveState(bool isActive)
    {
        if (IsActive == isActive)
            return;

        ActiveStackController = SpawnStackController();
        IsActive = isActive;
    }

    public void UnloadLevel()
    {
        _materialIndex = -1;
        IsActive = false;
        ActiveStackController = null;
        _previousStackController = null;
    }


    public StackController SpawnStackController()
    {
        StackController stackControllerObject = null;

        foreach (StackController stackController in StackControllers)
        {
            if (!stackController.gameObject.activeSelf)
            {
                stackControllerObject = stackController;
                break;
            }
        }

        if (stackControllerObject == null)
        {
            stackControllerObject = CreateStackController();
        }

        stackControllerObject.Initialize(_previousStackController, Positioner(), StackMaterial());
        stackControllerObject.OnCreateNewStack += StackControllerObject_OnCreateNewStack;
        stackControllerObject.OnFailed += StackControllerObject_OnFailed;

        _previousStackController = stackControllerObject;

        return stackControllerObject;
    }

    private void StackControllerObject_OnFailed()
    {
        OnFailed?.Invoke();
    }

    private void StackControllerObject_OnCreateNewStack()
    {
        ActiveStackController = SpawnStackController();
    }

    private StackController CreateStackController()
    {
        var stackControllerObject = Instantiate(stackControllerPrefab, transform);
        StackControllers.Add(stackControllerObject);
        return stackControllerObject;
    }

    public void StopLastStack()
    {
        ActiveStackController.StopStack();
    }

    public Material StackMaterial()
    {
        Material material = null;

        if (GameConfigs.Instance.RegularMaterial)
        {
            _materialIndex += 1;
            _materialIndex %= stackMaterials.Length - 1;
            material = stackMaterials[_materialIndex];
        }
        else
        {
            int materialIndex;
            do
            {
                materialIndex = UnityEngine.Random.Range(0, stackMaterials.Length);
            }
            while (_materialIndex == materialIndex);

            _materialIndex = materialIndex;
            material = stackMaterials[_materialIndex];
        }

        return material;
    }

    public PositionStatus Positioner()
    {
        PositionStatus status;

        if (!GameConfigs.Instance.RegularPosition)
        {
            int random = UnityEngine.Random.Range(0, 10);
            status = random % 2 == 0 ? PositionStatus.Left : PositionStatus.Right;
        }
        else
        {
            status = _lastPosition == PositionStatus.Left ? PositionStatus.Right : PositionStatus.Left;
        }

        _lastPosition = status;

        return status;
    }

    public enum PositionStatus
    {
        Left,
        Right
    }
}