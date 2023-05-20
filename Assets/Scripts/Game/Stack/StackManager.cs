using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [Header("References")]
    public StackController stackControllerPrefab;
    public List<StackController> StackControllers { get; private set; }
    public bool IsInitialized { get; private set; }

    public void Initialize()
    {
        IsInitialized = true;
    }

    public StackController SpawnStackController(Vector3 stackPosition, Vector3 stackScale)
    {
        StackController stackControllerObject = null;

        foreach (StackController stackController in StackControllers)
            if (stackController.gameObject.activeSelf) 
            {
                stackControllerObject = stackController;
                break;
            }

        if (stackControllerObject ==null)
            stackControllerObject = CreateStackController();

        stackControllerObject.Initialize(stackPosition, stackScale);

        return stackControllerObject;
    }
    private StackController CreateStackController()
    {
        var stackControllerObject = Instantiate(stackControllerPrefab, transform);
        StackControllers.Add(stackControllerObject);
        return stackControllerObject;
    }

    private void Update()
    {
        if (!IsInitialized)
            return;

    }
}
