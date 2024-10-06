using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    public PlayerBlockingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine){}
    public readonly int BlockingHash = Animator.StringToHash("Block");
    private const float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
        _stateMachine.Health.SetInvulnerable(true);
        _stateMachine.Animator.CrossFadeInFixedTime(BlockingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if(!_stateMachine.InputReader.IsBlocking)
        {
            ReturnToLocomotion();
            return;
        }
    }

    public override void Exit()
    {
        _stateMachine.Health.SetInvulnerable(false);
    }
}
