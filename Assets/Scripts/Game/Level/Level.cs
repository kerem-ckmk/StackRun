using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform firstStack;
    public FinishController finishController;
    public bool IsInitialized { get; private set; }

    public void Initialize(float stackCount)
    {
        finishController.Initialize(GameConfigs.Instance.StackScaleX, stackCount);
        firstStack.transform.SetLocalScaleZ(GameConfigs.Instance.StackScaleZ);
        IsInitialized = true;
    }
}
