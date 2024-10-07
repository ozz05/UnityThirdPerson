using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 _momentum;
    public PlayerFallingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        _momentum = _stateMachine.CharacterController.velocity;
        _momentum.y = 0;
        _stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(_momentum, deltaTime);
        
        if(_stateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
            return;
        }
        FaceTarget();
    }
    public override void Exit()
    {
    }
}
