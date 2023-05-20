using System.Collections.Generic;
using UnityEngine;
using System;
public class StackManager : MonoBehaviour
{
    [Header("References")]
    public Material[] stackMaterials;
    public StackController stackControllerPrefab;
    public List<StackController> StackControllers { get; private set; }
    public bool IsInitialized { get; private set; }
    public bool IsActive { get; private set; }
    public StackController LastStackController { get; private set; }

    private float _scaleX;
    private float _scaleZ;
    private bool _regularMaterial;
    private bool _regularPosition;
    private int _materialIndex = -1;
    private PositionStatus _lastPosition;
    public void Initialize()
    {
        StackControllers = new List<StackController>();
        StackControllers.Clear();

        _scaleX = GameConfigs.Instance.StackScaleX;
        _scaleZ = GameConfigs.Instance.StackScaleZ;
        _regularMaterial = GameConfigs.Instance.RegularMaterial;
        _regularPosition = GameConfigs.Instance.RegularPosition;

        IsInitialized = true;
    }

    public void SetActiveState(bool isActive)
    {
        if (IsActive == isActive)
            return;

        LastStackController = SpawnStackController(_scaleZ * 10f, _scaleX);
        IsActive = isActive;
    }

    public void UnloadLevel()
    {
        _materialIndex = -1;
        IsActive = false;
        LastStackController = null;
        _scaleX = GameConfigs.Instance.StackScaleX;
        _scaleZ = GameConfigs.Instance.StackScaleZ;
    }


    private void Update()
    {
        if (!IsInitialized || IsActive)
            return;

    }

    public StackController SpawnStackController(float positionZ, float scaleX)
    {
        StackController stackControllerObject = null;

        foreach (StackController stackController in StackControllers)
            if (!stackController.gameObject.activeSelf)
            {
                stackControllerObject = stackController;
                break;
            }

        if (stackControllerObject == null)
            stackControllerObject = CreateStackController();

        stackControllerObject.Initialize(positionZ, scaleX, _scaleZ, Positioner(), StackMaterial());
        stackControllerObject.OnCreateNewStack += StackControllerObject_OnCreateNewStack;
        return stackControllerObject;
    }

    private void StackControllerObject_OnCreateNewStack()
    {
        float positionZ = LastStackController.transform.position.z + _scaleZ * 10f;
        float scaleX = LastStackController.transform.localScale.x;

        LastStackController = SpawnStackController(positionZ, scaleX);
    }

    private StackController CreateStackController()
    {
        var stackControllerObject = Instantiate(stackControllerPrefab, transform);
        StackControllers.Add(stackControllerObject);
        return stackControllerObject;
    }

    public void StopLastStack()
    {
        LastStackController.StopStack();
    }

    public Material StackMaterial()
    {
        Material material = null;

        if (_regularMaterial)
        {
            _materialIndex += 1;
            _materialIndex %= stackMaterials.Length - 1;
            material = stackMaterials[_materialIndex];
        }
        else
        {
            int materialIndex; 
            do
                materialIndex = UnityEngine.Random.Range(0, stackMaterials.Length);
            while (_materialIndex == materialIndex);

            _materialIndex = materialIndex;
            material = stackMaterials[_materialIndex];
        }

        return material;
    }

    public PositionStatus Positioner()
    {
        PositionStatus status;

        if (!_regularPosition)
        {
            int random = UnityEngine.Random.Range(0, 10);
            status = random % 2 == 0 ? PositionStatus.Left : PositionStatus.Right;
        }
        else
            status = _lastPosition == PositionStatus.Left ? PositionStatus.Right : PositionStatus.Left;

        _lastPosition = status;

        return status;
    }

    public enum PositionStatus
    {
        Left,
        Right
    }

}
