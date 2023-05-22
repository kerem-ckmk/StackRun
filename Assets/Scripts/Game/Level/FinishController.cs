using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    public Transform visual;
    public Collider finishCollider;
    public bool IsInitialized { get; private set; }

    public void Initialize(float scaleX, float stackCount)
    {
        transform.SetLocalScaleX(scaleX);

        stackCount += 1;
        float positionZ = (stackCount * GameConfigs.Instance.StackScaleZ * 10f) + visual.transform.localScale.z * 0.5f;
        transform.SetPositionZ(positionZ);

        IsInitialized = true;
    }

    public void TriggerFinishCollider()
    {
        finishCollider.enabled = false;
    }
}
