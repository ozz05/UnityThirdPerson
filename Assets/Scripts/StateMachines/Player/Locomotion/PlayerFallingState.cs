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
        _stateMachine.LedgeDetector.OnLedgeDetect += LedgeDetected;
    }

    public override void Tick(float deltaTime)
    {
        Move(_momentum, deltaTime);
        
        if(_stateMachine.CharacterController.isGrounded || _stateMachine.CharacterController.velocity.y == 0)
        {
            ReturnToLocomotion();
            return;
        }
        FaceTarget();
    }
    public override void Exit()
    {
        
        _stateMachine.LedgeDetector.OnLedgeDetect -= LedgeDetected;
    }

    
    private void LedgeDetected(Vector3 ledgeForward)
    {
        _stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward));
    }
}
