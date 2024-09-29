using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}
    
    public override void Enter()
    {
        _stateMachine.InputReader.CancelEvent += OnCancel;
        _stateMachine.Animator.Play(TargetingBlendTreeHash);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.Targeter.CurrentTarget == null)
        {
            _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            return;
        }
        FaceTarget();
        UpdateAnimator(deltaTime);
        Move(CalculateMovement() * _stateMachine.TargetingMovementSpeed, deltaTime);
    }
    public override void Exit()
    {
        _stateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        _stateMachine.Targeter.Cancel();
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }
    private void UpdateAnimator(float deltaTime)
    {
        float normalizeY = _stateMachine.InputReader.MovementValue.y > 0 ? 1f : _stateMachine.InputReader.MovementValue.y < 0 ? -1f : 0;
        float normalizeX = _stateMachine.InputReader.MovementValue.x > 0 ? 1f : _stateMachine.InputReader.MovementValue.x < 0 ? -1f : 0;
        _stateMachine.Animator.SetFloat(TargetingForwardHash, normalizeY, 0.1f, deltaTime);
        _stateMachine.Animator.SetFloat(TargetingRightHash, normalizeX, 0.1f, deltaTime);
    }
    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += _stateMachine.transform.right * _stateMachine.InputReader.MovementValue.x;
        movement += _stateMachine.transform.forward * _stateMachine.InputReader.MovementValue.y;
        return movement;
    }
}
