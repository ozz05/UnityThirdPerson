using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine _stateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        _stateMachine = playerStateMachine;
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        _stateMachine.CharacterController.Move((motion + _stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void ReturnToLocomotion()
    {
        if (_stateMachine.Targeter.CurrentTarget == null)
        {
            _stateMachine.SwitchState(new PlayerFreeLookState(_stateMachine));
        }
        else
        {
            _stateMachine.SwitchState(new PlayerTargetingState(_stateMachine));
        }
    }

    protected void FaceTarget()
    {
        if (_stateMachine.Targeter.CurrentTarget == null) return;
        Vector3 direction = _stateMachine.Targeter.CurrentTarget.transform.position - _stateMachine.transform.position;
        direction.y = 0;
        _stateMachine.transform.rotation = Quaternion.LookRotation(direction);
    }
}
