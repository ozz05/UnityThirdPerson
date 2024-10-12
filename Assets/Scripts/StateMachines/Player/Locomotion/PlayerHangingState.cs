using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    public readonly int HangingHash = Animator.StringToHash("HangingIdle");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 _ledgeForward;
    private const float CooldownTime = 0.3f;
    private float _currentHangingTime;
    public PlayerHangingState(PlayerStateMachine playerStateMachine, Vector3 ledgeForward) : base(playerStateMachine)
    {
        _ledgeForward = ledgeForward;
    }

    public override void Enter()
    {
        _stateMachine.transform.rotation = Quaternion.LookRotation(_ledgeForward, Vector3.up);
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
    }

    public override void Exit()
    {
        _stateMachine.ForceReceiver.Reset();
    }

}
