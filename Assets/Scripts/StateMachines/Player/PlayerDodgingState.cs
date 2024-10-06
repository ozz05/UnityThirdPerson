using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    public readonly int DodgeHash = Animator.StringToHash("Dodge");
    private const string DODGE_ANIMATION_TAG = "Dodge";
    private const float CrossFadeDuration = 0.1f;
    private float _dodgeCurrentDuration = 0;
    private Vector3 _dodgingDirection = Vector3.zero;

    public PlayerDodgingState(PlayerStateMachine playerStateMachine, Vector3 dodgingDirection) : base(playerStateMachine)
    {
        _dodgingDirection = dodgingDirection;
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(DodgeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (_dodgeCurrentDuration >= _stateMachine.DodgeDuration)
        {
            _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            return;
        }
        else
        {
            FaceMovementDirection();
            Move(_stateMachine.transform.forward * _stateMachine.TargetingMovementSpeed, deltaTime);
            _dodgeCurrentDuration += deltaTime;
        }
    }
    public override void Exit()
    {
        
    }
    protected void FaceMovementDirection()
    {
        _stateMachine.transform.rotation = Quaternion.LookRotation(_dodgingDirection);
    }
}
