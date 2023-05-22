using DG.Tweening;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const string IDLE_ANIMATION = "Idle";
    private const string RUN_ANIMATION = "Run";
    private const string WALK_ANIMATION = "Walk";
    private const string DANCE_ANIMATION = "Dance";
    private const string FALL_ANIMATION = "Fall";

    [Header("References")]
    public Animator animator;
    public Rigidbody playerRigidbody;
    public bool IsInitialized { get; private set; }
    public bool IsActive { get; private set; }

    private AnimationState _currentAnimationState;
    private Vector3 _targetPosition;
    private Sequence _finishSequence;
    private Sequence _failSequence;
    private bool _fail;

    public event Action WinPlayer;

    public void Initialize()
    {
        _currentAnimationState = AnimationState.Idle;
        IsInitialized = true;
    }

    public void Prepare()
    {
        _targetPosition = Vector3.forward * 10f * GameConfigs.Instance.StackScaleZ;
        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        ChangeAnimationState(AnimationState.Idle);
    }

    public void StartGameplay()
    {
        SetActiveState(true);
        ChangeAnimationState(AnimationState.Run);
    }

    public void FailedGameplay()
    {
        _fail = true;
    }

    public void SetActiveState(bool isActive)
    {
        if (IsActive == isActive)
            return;

        IsActive = isActive;
    }

    public void SetTransformCenter(Vector3 center)
    {
        _targetPosition = center;
    }

    public void UnloadLevel()
    {
        _fail = false;
        playerRigidbody.isKinematic = true;
        _failSequence?.Kill();
        _finishSequence?.Kill();
        IsActive = false;
        animator.Rebind();
        animator.Update(0f);
    }

    private void Update()
    {
        if (!IsInitialized || !IsActive)
            return;


    }

    private void FixedUpdate()
    {
        if (!IsInitialized || !IsActive)
            return;

        Vector3 direction = _targetPosition - transform.position;
        float moveSpeed = Time.fixedDeltaTime * GameConfigs.Instance.PlayerMoveSpeed;
        Vector3 newPosition = transform.position + direction.normalized * moveSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerRigidbody.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed);
        playerRigidbody.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == TagsAndLayers.StackStartIndex)
        {
            var stackController = other.GetComponentInParent<StackController>();
            stackController.TriggeredStartCollider();
            _targetPosition += Vector3.forward * GameConfigs.Instance.StackScaleZ * 15f;
        }

        if (other.gameObject.layer == TagsAndLayers.FinishIndex)
        {
            var finishController = other.GetComponentInParent<FinishController>();
            finishController.TriggerFinishCollider();
            SetActiveState(false);
            FinishGame();
        }

        if (other.gameObject.layer == TagsAndLayers.StackEndIndex)
        {
            if (!_fail)
                return;

            _fail = true;
            var stackController = other.GetComponentInParent<StackController>();
            stackController.TriggeredEndCollider();

            FallPlayer();
        }

        if (other.gameObject.layer == TagsAndLayers.FirstPlatformIndex)
        {
            if (!_fail)
                return;

            _fail = true;
            var otherCollider = other.GetComponent<Collider>();
            otherCollider.enabled = false;

            FallPlayer();
        }
    }

    private void FallPlayer()
    {
        SetActiveState(false);

        float _targetZ = transform.position.z + (5f * GameConfigs.Instance.StackScaleZ);
        _failSequence?.Kill();
        _failSequence = DOTween.Sequence();
        _failSequence.Append(transform.DOMoveZ(_targetZ, 0.65f).SetEase(Ease.Linear));
        _failSequence.AppendCallback(() =>
        {
            playerRigidbody.isKinematic = false;
            ChangeAnimationState(AnimationState.Fall);
        });
    }

    private void FinishGame()
    {
        float newPositionZ = transform.position.z + (2f * GameConfigs.Instance.StackScaleZ);

        _finishSequence?.Kill();
        _finishSequence = DOTween.Sequence();
        _finishSequence.Append(transform.DOMoveZ(newPositionZ, 0.4f).SetEase(Ease.Linear));
        _finishSequence.AppendCallback(() => ChangeAnimationState(AnimationState.Dance));
        _finishSequence.Append(transform.DORotate(Vector3.up * 180f, 0.4f).SetEase(Ease.Linear));
        _finishSequence.Play();

        WinPlayer?.Invoke();
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        if (_currentAnimationState == newState)
            return;

        animator.SetBool(RUN_ANIMATION, false);
        animator.SetBool(WALK_ANIMATION, false);
        animator.SetBool(IDLE_ANIMATION, false);
        animator.ResetTrigger(DANCE_ANIMATION);
        animator.ResetTrigger(FALL_ANIMATION);

        switch (newState)
        {
            case AnimationState.Idle:
                animator.SetBool(IDLE_ANIMATION, true);
                break;
            case AnimationState.Run:
                animator.SetBool(RUN_ANIMATION, true);
                break;
            case AnimationState.Walk:
                animator.SetBool(WALK_ANIMATION, true);
                break;
            case AnimationState.Dance:
                animator.SetTrigger(DANCE_ANIMATION);
                break;
            case AnimationState.Fall:
                animator.SetTrigger(FALL_ANIMATION);
                break;
        }

        _currentAnimationState = newState;
    }
}

public enum AnimationState
{
    Idle,
    Run,
    Walk,
    Dance,
    Fall
}