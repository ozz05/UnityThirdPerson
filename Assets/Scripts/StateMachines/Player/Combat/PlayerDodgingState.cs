using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    public readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private const string DODGE_ANIMATION_TAG = "Dodge";
    private const float CrossFadeDuration = 0.1f;
    private float _dodgeCurrentDuration = 0;
    private Vector2 _dodgingDirection = Vector2.zero;

    public PlayerDodgingState(PlayerStateMachine playerStateMachine, Vector2 dodgingDirection) : base(playerStateMachine)
    {
        _dodgingDirection = dodgingDirection;
    }

    public override void Enter()
    {
        _stateMachine.Animator.SetFloat(DodgeForwardHash, _dodgingDirection.y);
        _stateMachine.Animator.SetFloat(DodgeRightHash, _dodgingDirection.x);
        _stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);
        _stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        if (_dodgeCurrentDuration >= _stateMachine.DodgeDuration)
        {
            ReturnToLocomotion();
            return;
        }
        else
        {
            Move(CalculateMovement(deltaTime), deltaTime);
            FaceTarget();
            _dodgeCurrentDuration += deltaTime;
        }
    }
    public override void Exit()
    {
        _stateMachine.Health.SetInvulnerable(false);
    }
    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += _stateMachine.transform.right * _dodgingDirection.x * _stateMachine.DodgeDistance / _stateMachine.DodgeDuration;
        movement += _stateMachine.transform.forward * _dodgingDirection.y * _stateMachine.DodgeDistance / _stateMachine.DodgeDuration;

        return movement;
    }
}
