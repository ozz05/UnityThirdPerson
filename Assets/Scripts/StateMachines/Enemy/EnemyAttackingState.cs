using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    public readonly int AttackHash = Animator.StringToHash("Attack");
    private const string ATTACK_ANIMATION_TAG = "Attack";
    private Attack _attack;
    private const float CrossFadeDuration = 0.2f;
    private bool _forceApplied = false;
    public EnemyAttackingState(EnemyStateMachine enemyStateMachine, int attackIndex) : base(enemyStateMachine)
    {
        if (attackIndex >= _stateMachine.Attacks.Length) return;
        _attack = _stateMachine.Attacks[attackIndex];
        _stateMachine.Weapon.SetWeaponDamage(_attack.Damage, _attack.Knockback);
    }

    public override void Enter()
    {
        FacePlayer();
        _stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        float normalizedTime = GetNormalizedTime(_stateMachine.Animator, ATTACK_ANIMATION_TAG);
        if (normalizedTime >= 1)
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
        }
    }

    public override void Exit(){}
    private void TryApplyForce()
    {
        if (_forceApplied) return;
        _forceApplied = true;
        _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * _attack.Force);
    }
}
