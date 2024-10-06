using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine){}

    public override void Enter()
    {
        _stateMachine.Ragdoll.ToggleRagdoll(true);
        _stateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(_stateMachine.Target);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
