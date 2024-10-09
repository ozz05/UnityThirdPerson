using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}
    private const float CrossFadeDuration = 0.2f;
    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnCancel;
        _stateMachine.InputReader.DodgeEvent += OnDodge;
        _stateMachine.InputReader.JumpEvent += OnJump;
        _stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine, 0));
            return;
        }
        if (_stateMachine.InputReader.IsBlocking)
        {
            _stateMachine.SwitchState(new PlayerBlockingState(_stateMachine));
            return;
        }
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
        _stateMachine.InputReader.TargetEvent -= OnCancel;
        _stateMachine.InputReader.DodgeEvent -= OnDodge;
        _stateMachine.InputReader.JumpEvent -= OnJump;
    }
    private void OnDodge()
    {
        if (_stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            return;
        }
        _stateMachine.SwitchState(new PlayerDodgingState(_stateMachine, _stateMachine.InputReader.MovementValue));
    }
    private void OnCancel()
    {
        _stateMachine.Targeter.Cancel();
        _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
    }
    private void OnJump()
    {
        if(!_stateMachine.CharacterController.isGrounded) return;
        
        _stateMachine.SwitchState(new PlayerJumpingState(_stateMachine));
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
