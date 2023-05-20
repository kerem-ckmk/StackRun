using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public bool IsInitialized { get; private set; }

    public void Initialize(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
        IsInitialized = true;
    }
}
