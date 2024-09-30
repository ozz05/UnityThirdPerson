using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack _attack;
    public PlayerAttackingState(PlayerStateMachine playerStateMachine, int attackId) : base(playerStateMachine)
    {
        if (attackId >= _stateMachine.Attacks.Length) return;
        _attack = _stateMachine.Attacks[attackId];
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(_attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        if (!_stateMachine.InputReader.IsAttacking)
        {
            // stop attacking
            return;
        }
    }

    public override void Exit()
    {
        
    }

}
