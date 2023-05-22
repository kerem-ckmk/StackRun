using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.PackageManager;
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

    public void Initialize()
    {
        _currentAnimationState = AnimationState.Idle;
        IsInitialized = true;
    }

    public void Prepare()
    {
        _targetPosition = Vector3.forward * 10f;
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
        SetActiveState(false);
        ChangeAnimationState(AnimationState.Idle);
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
        IsActive = false;
        animator.Rebind();
        animator.Update(0f);
    }

    private void Update()
    {
        if (!IsInitialized || !IsActive)
            return;

        Vector3 direction = _targetPosition - transform.position;
        float moveSpeed = Time.deltaTime * GameConfigs.Instance.PlayerMoveSpeed;
        Vector3 newPosition = transform.position + direction.normalized * moveSpeed;

        Quaternion targetRotation= Quaternion.LookRotation(direction);
        playerRigidbody.rotation = Quaternion.Lerp(transform.rotation, targetRotation, moveSpeed);
        playerRigidbody.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == TagsAndLayers.StackStartIndex)
        {
            var stackController = other.GetComponentInParent<StackController>();
            stackController.TriggeredStartCollider();
            _targetPosition += Vector3.forward * 10f;
        }
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