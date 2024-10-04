using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine){}
    public readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    public readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f; 

    public override void Enter()
    {
        _stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        MoveToPlayer(deltaTime);
        if (!IsInChaseRage())
        {
            _stateMachine.SwitchState(new EnemyIdleState(_stateMachine));
            return;
        }
        else if(IsInAttackingRange())
        {
            _stateMachine.SwitchState( new EnemyAttackingState(_stateMachine, 0));
            return;
        }
        _stateMachine.Animator.SetFloat(SpeedHash, 1, AnimatorDampTime, deltaTime);
        FacePlayer();
    }

    public override void Exit()
    {
        _stateMachine.NavMeshAgent.ResetPath();
        _stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        _stateMachine.NavMeshAgent.destination = _stateMachine.Player.transform.position;

        Move(_stateMachine.NavMeshAgent.desiredVelocity.normalized * _stateMachine.MovementSpeed, deltaTime);
        _stateMachine.NavMeshAgent.velocity = _stateMachine.CharacterController.velocity;
    }
}
