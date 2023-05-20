using System;
using UnityEngine;
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
    private float _speed;
    private StackManager.PositionStatus _currentPositionStatus;
    private Material _material;
    public event Action OnCreateNewStack;
    

    public void Initialize(float positionZ, float scaleX, float scaleZ, StackManager.PositionStatus positionStatus, Material stackMaterial)
    {
        _material = stackMaterial;
        visualRenderer.sharedMaterial = _material;
        _currentPositionStatus = positionStatus;
        ChoosePosition(positionZ);

         _speed = GameConfigs.Instance.PlayerMoveSpeed * 2f;

        startCollider.transform.localScale = new Vector3(scaleX, 1f, 1f);
        startCollider.enabled = true;
        endCollider.transform.localScale = new Vector3(scaleX, 1f, 1f);
        endCollider.enabled = true;

        stackVisual.localScale = new Vector3(scaleX, 1f, scaleZ);

        IsStop = false;
        IsInitialized = true;
    }

    public void StopStack()
    {
        IsStop = true;
    }

    public void ChoosePosition(float positionZ)
    {
        float positionX = _currentPositionStatus == StackManager.PositionStatus.Left ? 
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

        transform.position = Vector3.Lerp(transform.position, transform.position + _direction, Time.deltaTime * _speed);
    }
}

