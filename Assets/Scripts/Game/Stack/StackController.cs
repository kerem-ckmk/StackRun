using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static StackManager;

public class StackController : MonoBehaviour
{
    public Transform stackVisual;
    public Renderer visualRenderer;
    public Collider startCollider;
    public Collider endCollider;
    public bool IsInitialized { get; private set; }
    public bool IsStop { get; private set; }

    private Vector3 _direction;
    private PositionStatus _currentPositionStatus;
    private Material _material;
    private StackController _previousStackController;

    public event Action OnCreateNewStack;

    public void Initialize(StackController previousStackController, PositionStatus positionStatus, Material stackMaterial)
    {
        _previousStackController = previousStackController;
        _material = stackMaterial;
        visualRenderer.sharedMaterial = _material;
        _currentPositionStatus = positionStatus;

        CalculateTransform();

        IsStop = false;
        IsInitialized = true;
    }

    public void StopStack()
    {
        IsStop = true;

        CalculateExcess();
    }

    public void CalculateTransform()
    {
        Vector3 previousScale;
        float positionZ;

        if (_previousStackController == null)
        {
            previousScale = new Vector3(GameConfigs.Instance.StackScaleX, stackVisual.localScale.y, GameConfigs.Instance.StackScaleZ);
            positionZ = previousScale.z * 10f;
        }
        else
        {
            previousScale = _previousStackController.stackVisual.localScale;
            positionZ = _previousStackController.transform.position.z + GameConfigs.Instance.StackScaleZ * 10f;
        }

        startCollider.transform.localScale = new Vector3(previousScale.x, 1f, 1f);
        startCollider.enabled = true;
        endCollider.transform.localScale = new Vector3(previousScale.x, 1f, 1f);
        endCollider.enabled = true;
        stackVisual.localScale = new Vector3(previousScale.x, 1f, previousScale.z);

        float positionX = _currentPositionStatus == PositionStatus.Left ?
            -GameConfigs.Instance.DistanceCenter : GameConfigs.Instance.DistanceCenter;

        _direction = _currentPositionStatus == PositionStatus.Left ? transform.right : -transform.right;

        transform.position = new Vector3(positionX, 0f, positionZ);
    }

    public void TriggeredStartCollider()
    {
        startCollider.enabled = false;
        OnCreateNewStack?.Invoke();
    }

    private void Update()
    {
        if (!IsInitialized || IsStop)
            return;

        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, Time.deltaTime * GameConfigs.Instance.StackMoveSpeed);
    }

    public void ShrinkObject(float excessDistance)
    {
        //float shrinkFactor = 1f - excessDistance / stackVisual.localScale.x;
        //stackVisual.localScale = new Vector3(stackVisual.localScale.x * shrinkFactor, stackVisual.localScale.y, stackVisual.localScale.z);
        //transform.position = new Vector3(transform.position.x - excessDistance / 2f, transform.position.y, transform.position.z);
    }

    public void CalculateExcess()
    {
        float centerX = transform.position.x * 0.1f;
        float previousCenterX = _previousStackController == null ? 0f : _previousStackController.transform.position.x * 0.1f;
        float excess = previousCenterX - centerX;

        float newScaleX = stackVisual.localScale.x - Mathf.Abs(excess);
        stackVisual.SetLocalScaleX(newScaleX);

        float newPositionX = transform.position.x;

        if (excess < 0f)
            newPositionX -= Mathf.Abs(excess) * 10f * 0.5f;
        else
            newPositionX += Mathf.Abs(excess) * 10f * 0.5f;

        transform.SetPositionX(newPositionX);
    }
}