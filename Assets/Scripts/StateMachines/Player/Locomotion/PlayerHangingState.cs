using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    public readonly int HangingHash = Animator.StringToHash("HangingIdle");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 _ledgeForward;
    private Vector3 _closestPoint;
    private const float CooldownTime = 0.3f;
    private float _currentHangingTime;

    public PlayerHangingState(PlayerStateMachine playerStateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(playerStateMachine)
    {
        _ledgeForward = ledgeForward;
        _closestPoint = closestPoint;
    }

    public override void Enter()
    {
        _stateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);
        // set the player to match the closest position to the LedgeDetector/ Hands
        _stateMachine.CharacterController.enabled = false;// disable the character controller to be hable to change the position using transform
        _stateMachine.transform.position = _closestPoint - (_stateMachine.LedgeDetector.transform.position - _stateMachine.transform.position);
        _stateMachine.CharacterController.enabled = true;
        _stateMachine.Animator.CrossFadeInFixedTime(HangingHash, CrossFadeDuration);
        _currentHangingTime = 0f;
    }

    public override void Tick(float deltaTime)
    {
        _currentHangingTime += deltaTime;
        if(_currentHangingTime < CooldownTime) return;
        if (_stateMachine.InputReader.MovementValue.y < 0)
        {
            _stateMachine.SwitchState(new PlayerFallingState(_stateMachine));
        }
        else if (_stateMachine.InputReader.MovementValue.y > 0)
        {
            _stateMachine.SwitchState(new PlayerPullUpState(_stateMachine));
        }
    }

    public override void Exit()
    {
        _stateMachine.CharacterController.Move(Vector3.zero);
        _stateMachine.ForceReceiver.Reset();
    }

}
