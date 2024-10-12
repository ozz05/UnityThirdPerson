using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    public readonly int PullUpHash = Animator.StringToHash("PullUp");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 _offset = new Vector3(0f, 2.35f, 0.65f);
    public PlayerPullUpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);

    }

    public override void Tick(float deltaTime)
    {
        if(_stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) return;
        _stateMachine.CharacterController.enabled = false;
        _stateMachine.transform.Translate(_offset);
        _stateMachine.CharacterController.enabled = true;
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine, false));
    }

    public override void Exit()
    {
        _stateMachine.CharacterController.Move(Vector3.zero);
        _stateMachine.ForceReceiver.Reset();
    }
}
