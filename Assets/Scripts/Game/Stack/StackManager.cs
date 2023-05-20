using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public void Initialize()
    {
        _scaleX = GameConfigs.Instance.StackScaleX;
        _scaleZ = GameConfigs.Instance.StackScaleZ;
        IsInitialized = true;
    }

    public void SetActiveState(bool isActive)
    {
        if (IsActive == isActive)
            return;

        LastStackController = SpawnStackController();
        IsActive = isActive;
    }

    public void UnloadLevel()
    {
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



    public StackController SpawnStackController()
    {
        StackController stackControllerObject = null;

        foreach (StackController stackController in StackControllers)
            if (stackController.gameObject.activeSelf)
            {
                stackControllerObject = stackController;
                break;
            }

        if (stackControllerObject == null)
            stackControllerObject = CreateStackController();

        stackControllerObject.Initialize(5f, _scaleX, _scaleZ);

        return stackControllerObject;
    }
    private StackController CreateStackController()
    {
        var stackControllerObject = Instantiate(stackControllerPrefab, transform);
        StackControllers.Add(stackControllerObject);
        return stackControllerObject;
    }

}
