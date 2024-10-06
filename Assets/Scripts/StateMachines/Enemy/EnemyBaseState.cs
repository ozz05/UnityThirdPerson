using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine _stateMachine;
    public EnemyBaseState (EnemyStateMachine enemyStateMachine)
    {
        _stateMachine = enemyStateMachine;
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        _stateMachine.CharacterController.Move((motion + _stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void FacePlayer()
    {
        if (_stateMachine.Player == null) return;

        Vector3 playerDirection = _stateMachine.Player.transform.position - _stateMachine.transform.position;
        playerDirection.y = 0;
        _stateMachine.transform.rotation = Quaternion.LookRotation(playerDirection);
    }
    protected bool IsInChaseRage()
    {
        if (!_stateMachine.Player.IsAlive)
        {
            return false;
        }
        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;// square magnitude gives the magnitude squared whitch is more optimized, since the actual magnitud performs a square operation
        return playerDistanceSqr <= (_stateMachine.PlayerChaseRange * _stateMachine.PlayerChaseRange);// you have to get the player chase Range square to be able to use the sqrMagnitude
    }
    protected bool IsInAttackingRange()
    {
        if (!_stateMachine.Player.IsAlive)
        {
            return false;
        }
        float playerDistanceSqr = (_stateMachine.Player.transform.position - _stateMachine.transform.position).sqrMagnitude;// square magnitude gives the magnitude squared whitch is more optimized, since the actual magnitud performs a square operation
        return playerDistanceSqr <= (_stateMachine.AttackingRange * _stateMachine.AttackingRange);// you have to get the player chase Range square to be able to use the sqrMagnitude
    }
}
