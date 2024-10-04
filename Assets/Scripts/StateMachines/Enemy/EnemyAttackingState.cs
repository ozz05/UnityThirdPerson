using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    public readonly int AttackHash = Animator.StringToHash("Attack");
    private const string ATTACK_ANIMATION_TAG = "Attack";
    private Attack _attack;
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.2f;
    private bool _forceApplied = false;
    public EnemyAttackingState(EnemyStateMachine enemyStateMachine, int attackIndex) : base(enemyStateMachine)
    {
        if (attackIndex >= _stateMachine.Attacks.Length) return;
        _attack = _stateMachine.Attacks[attackIndex];
        _stateMachine.Weapon.SetWeaponDamage(_attack.Damage);
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime();
        if (!IsInAttackingRange())
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
            return;
        }
        else
        {
            if (normalizedTime < 1)
            {
                if (normalizedTime >= _attack.ForceTime)
                {
                    TryApplyForce();
                }
            }
            else
            {
                _stateMachine.SwitchState(new EnemyAttackingState(_stateMachine, Mathf.Max(_attack.ComboStateIndex, 0)));
            }
        }
    }

    public override void Exit(){}

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
}
