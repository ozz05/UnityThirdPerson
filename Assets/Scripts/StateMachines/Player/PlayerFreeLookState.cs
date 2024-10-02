using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {}
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.2f; 

    public override void Enter()
    {
        _stateMachine.InputReader.TargetEvent += OnTarget;
        _stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        if (_stateMachine.InputReader.IsAttacking)
        {
            _stateMachine.SwitchState(new PlayerAttackingState(_stateMachine, 0));
            return;
        }
        Vector3 movement = CalculateMovement();
        MovePlayer(movement, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputReader.TargetEvent -= OnTarget;
    }
    private void OnTarget()
    {
        if (!_stateMachine.Targeter.SelectTarget()) return;
        _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
    }
    private void MovePlayer(Vector3 movement, float deltaTime)
    {
        Move(movement * _stateMachine.FreeLookMovementSpeed, deltaTime);
        if (_stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            _stateMachine.Animator.SetFloat(FreeLookHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        _stateMachine.Animator.SetFloat(FreeLookHash, 1, AnimatorDampTime, deltaTime);
    }
    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        if(movement == Vector3.zero) return;
        _stateMachine.transform.rotation = Quaternion.Lerp(
            _stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            _stateMachine.RotationDamping * deltaTime
        );
    }
    private Vector3 CalculateMovement()
    {
        Vector3 forward = _stateMachine.MainCameraTransform.forward;
        Vector3 right = _stateMachine.MainCameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        return forward * _stateMachine.InputReader.MovementValue.y + 
            right * _stateMachine.InputReader.MovementValue.x;
    }
}
