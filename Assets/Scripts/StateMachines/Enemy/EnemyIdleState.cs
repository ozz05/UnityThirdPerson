using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    public readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.2f; 
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine){}

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
        
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInChaseRage())
        {
            _stateMachine.SwitchState(new EnemyChasingState(_stateMachine));
            return;
        }
        _stateMachine.Animator.SetFloat(SpeedHash, 0, AnimatorDampTime, deltaTime);
    }
    public override void Exit()
    {
        
    }
}
