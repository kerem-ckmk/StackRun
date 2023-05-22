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
    public CutObjectController cutObjectController;
    public bool IsInitialized { get; private set; }
    public bool IsStop { get; private set; }

    private Vector3 _direction;
    private PositionStatus _currentPositionStatus;
    private Material _material;
    private StackController _previousStackController;

    public event Action OnCreateNewStack;
    public event Action<Vector3> NewStackCenter;
    public event Action OnFailed;

    public void Initialize(StackController previousStackController, PositionStatus positionStatus, Material stackMaterial)
    {
        cutObjectController.gameObject.SetActive(false);

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

        CreateCutObject();
    }

    private void OnDestroy()
    {
        OnCreateNewStack = null;
        OnFailed = null;
        NewStackCenter = null;
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
        cutObjectController.transform.SetLocalScaleZ(previousScale.z);
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

    public void CreateCutObject()
    {
        float centerX = transform.position.x * 0.1f;
        float previousCenterX = _previousStackController == null ? 0f : _previousStackController.transform.position.x * 0.1f;
        float previousScaleX = _previousStackController == null ? 1f : _previousStackController.stackVisual.localScale.x;
        float excess = previousCenterX - centerX;

        if (Mathf.Abs(excess) > previousScaleX)
        {
            HandleExcess(excess);
            return;
        }

        float newScaleX = stackVisual.localScale.x - Mathf.Abs(excess);
        stackVisual.SetLocalScaleX(newScaleX);
        float newPositionX = CalculateNewPositionX(excess);
        float cutObjectPositionX = CalculateCutObjectPositionX(excess, newScaleX, newPositionX);

        transform.SetPositionX(newPositionX);
        NewStackCenter?.Invoke(transform.position);

        cutObjectController.SetTransform(Mathf.Abs(excess), cutObjectPositionX, _material);

    }

    private void HandleExcess(float excess)
    {
        cutObjectController.SetTransform(stackVisual.localScale.x, transform.position.x, _material);
        stackVisual.gameObject.SetActive(false);
        OnFailed?.Invoke();
    }

    private float CalculateNewPositionX(float excess)
    {
        float newPositionX = transform.position.x;

        if (excess < 0f)
            newPositionX -= Mathf.Abs(excess) * 10f * 0.5f;
        else
            newPositionX += Mathf.Abs(excess) * 10f * 0.5f;

        return newPositionX;
    }

    private float CalculateCutObjectPositionX(float excess, float newScaleX, float newPositionX)
    {
        float cutObjectPositionX;

        if (excess < 0f)
            cutObjectPositionX = newPositionX + (newScaleX * 0.5f * 10f) + Mathf.Abs(excess * 0.5f * 10f);
        else
            cutObjectPositionX = newPositionX - (newScaleX * 0.5f * 10f) - Mathf.Abs(excess * 0.5f * 10f);
        
        return cutObjectPositionX;
    }
}