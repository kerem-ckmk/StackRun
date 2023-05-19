using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public bool IsInitialized { get; private set; }

    public void Initialize()
    {

        IsInitialized = true;
    }
}
