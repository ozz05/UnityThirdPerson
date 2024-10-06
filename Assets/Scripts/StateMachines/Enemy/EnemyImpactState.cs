using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    public readonly int ImpactHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.1f;
    private float _duration = 0;

    public EnemyImpactState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        _duration += deltaTime;
        if (_duration >= _stateMachine.ImpactDuration)
        {
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}
