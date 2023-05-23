using System;
using UnityEngine;
using static StackManager;
using DG.Tweening;

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
    private bool _failed;
    private Sequence _sequence;

    public event Action OnCreateNewStack;
    public event Action<Vector3, bool> NewStackCenter;
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
        _sequence?.Kill();
        _sequence = null;
        OnCreateNewStack = null;
        OnFailed = null;
        NewStackCenter = null;
    }

    private void CalculateTransform()
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

        startCollider.enabled = true;
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

    public void TriggeredEndCollider()
    {
        endCollider.enabled = false;
    }

    private void Update()
    {
        if (!IsInitialized || IsStop)
            return;

        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, Time.deltaTime * GameConfigs.Instance.StackMoveSpeed);

        if (_failed)
            return;

        bool checkFail = CheckFail();

        if (checkFail)
            HandleExcess();
    }

    private bool CheckFail()
    {
        bool fail = false;
        float excess = CalculateExcess();

        if (_currentPositionStatus == PositionStatus.Right)
        {
            if (excess > stackVisual.localScale.x)
                fail = true;
        }
        else if (_currentPositionStatus == PositionStatus.Left)
        {
            if (excess < 0 && Mathf.Abs(excess) > stackVisual.localScale.x)
                fail = true;
        }

        return fail;
    }

    private void CreateCutObject()
    {
        float previousScaleX = _previousStackController == null ? 1f : _previousStackController.stackVisual.localScale.x;
        float excess = CalculateExcess();
        float newPositionX;

        if (Mathf.Abs(excess) > previousScaleX)
        {
            HandleExcess();
            return;
        }

        if (Mathf.Abs(excess) < 0.03f)
        {
            DockingAnimation();
            newPositionX = _previousStackController == null ? 0f : _previousStackController.transform.position.x;
            NewStackCenter?.Invoke(transform.position, true);
            transform.SetPositionX(newPositionX);
            return;
        }

        float newScaleX = stackVisual.localScale.x - Mathf.Abs(excess);
        stackVisual.SetLocalScaleX(newScaleX);
        newPositionX = CalculateNewPositionX(excess);
        float cutObjectPositionX = CalculateCutObjectPositionX(excess, newScaleX, newPositionX);

        transform.SetPositionX(newPositionX);
        NewStackCenter?.Invoke(transform.position, false);

        cutObjectController.SetTransform(Mathf.Abs(excess), cutObjectPositionX, _material);

        DockingAnimation();
    }

    private void DockingAnimation()
    {
        float firstPositionY = stackVisual.localPosition.y - 1f;
        float lastPositionY = stackVisual.localPosition.y;

        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(stackVisual.DOLocalMoveY(firstPositionY, 0.4f).SetEase(Ease.OutQuint));
        _sequence.Append(stackVisual.DOLocalMoveY(lastPositionY, 0.2f).SetEase(Ease.InQuint));
        _sequence.Play();
    }

    private void HandleExcess()
    {
        if (_failed)
            return;

        _failed = true;
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

    private float CalculateExcess()
    {
        float centerX = transform.position.x * 0.1f;
        float previousCenterX = _previousStackController == null ? 0f : _previousStackController.transform.position.x * 0.1f;
        float excess = previousCenterX - centerX;

        return excess;
    }
}