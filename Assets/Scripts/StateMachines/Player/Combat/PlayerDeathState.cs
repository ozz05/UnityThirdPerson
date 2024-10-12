using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}

    public override void Enter()
    {
        
        _stateMachine.Ragdoll.ToggleRagdoll(true);
        _stateMachine.Weapon.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        
    }
}
