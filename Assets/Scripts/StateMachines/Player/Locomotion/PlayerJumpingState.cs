using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public readonly int JumpHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 _momentum;

    public PlayerJumpingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        _stateMachine.LedgeDetector.OnLedgeDetect += LedgeDetected;
        _stateMachine.ForceReceiver.Jump(_stateMachine.JumpForce);
        
        _momentum = _stateMachine.CharacterController.velocity;
        _momentum.y = 0;
        _stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(_momentum, deltaTime);
        
        if(_stateMachine.CharacterController.velocity.y <= 0)
        {
            _stateMachine.SwitchState(new PlayerFallingState(_stateMachine));
            return;
        }
        FaceTarget();
    }
    
    public override void Exit()
    {
        _stateMachine.LedgeDetector.OnLedgeDetect -= LedgeDetected;
    }
    
    private void LedgeDetected(Vector3 ledgeForward, Vector3 closestPoint)
    {
        _stateMachine.SwitchState(new PlayerHangingState(_stateMachine, ledgeForward, closestPoint));
    }
}
