using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform firstStack;
    public bool IsInitialized { get; private set; }

    public void Initialize()
    {
        firstStack.transform.SetLocalScaleZ(GameConfigs.Instance.StackScaleZ);
        IsInitialized = true;
    }
}
