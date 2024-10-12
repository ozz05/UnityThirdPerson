using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;
    private const string ATTACK_ANIMATION_TAG = "Attack";
    private bool _forceApplied; 
    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackIndex) : base(playerStateMachine)
    {
        if (attackIndex >= _stateMachine.Attacks.Length) return;
        _attack = _stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, _attack.TransitionTime);
        _stateMachine.Weapon.SetWeaponDamage(_attack.Damage, _attack.Knockback);
    }

    public override void Tick(float deltaTime)
    {
        
        float normalizedTime = GetNormalizedTime(_stateMachine.Animator, ATTACK_ANIMATION_TAG);
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
            ReturnToLocomotion();
        }
        Move(deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        
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
