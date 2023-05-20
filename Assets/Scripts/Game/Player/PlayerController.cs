using System.Collections;
using System.Collections.Generic;
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
    public Vector3 Direction { get; private set; }
    public bool IsActive { get; private set; }
    private AnimationState _currentAnimationState;

    public void Initialize()
    {
        _currentAnimationState = AnimationState.Idle;
        IsInitialized = true;
    }

    public void SetActiveState(bool isActive)
    {
        if (IsActive == isActive)
            return;

        ChangeAnimationState(AnimationState.Run);
        IsActive = isActive;
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

        Vector3 targetPosition = transform.position + transform.forward;
        targetPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * GameConfigs.Instance.PlayerMoveSpeed);
        playerRigidbody.MovePosition(targetPosition);
    }

    private void ChangeAnimationState(AnimationState newState)
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