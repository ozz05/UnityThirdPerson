using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;
    private const string ATTACK_ANIMATION_TAG = "Attack";
    private float _previousFrameTime;
    private bool _forceApplied; 
    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine)
    {
        if (attackIndex >= _stateMachine.Attacks.Length) return;
        _attack = _stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionTime);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();
        float normalizedTime = GetNormalizedTime();
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= _attack.ForceTime)
            {
                TryApplyForce();
            }
            if (_stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (_stateMachine.Targeter.CurrentTarget == null)
            {
                _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
            }
            else
            {
                _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
            }
        }
        _previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = _stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = _stateMachine.Animator.GetNextAnimatorStateInfo(0);
        if (_stateMachine.Animator.IsInTransition(0) && nextInfo.IsTag(ATTACK_ANIMATION_TAG))
        {
            return nextInfo.normalizedTime;
        }
        else if (!_stateMachine.Animator.IsInTransition(0) && currentInfo.IsTag(ATTACK_ANIMATION_TAG))
        {
            return currentInfo.normalizedTime;
        }
        return 0;
    }
    private void TryApplyForce()
    {
        if (_forceApplied) return;
        _forceApplied = true;
        _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * _attack.Force);
    }
    private void TryComboAttack(float normalizedTime)
    {
        if (_attack.ComboStateIndex < 0 || normalizedTime < _attack.ComboAttackTime) return;
        if (_attack.ComboStateIndex < _stateMachine.Attacks.Length)
        {
            _stateMachine.SwitchState
            (
                new PlayerAttackingState
                (
                    _stateMachine,
                    _attack.ComboStateIndex
                )
            );
        }
    }
}
