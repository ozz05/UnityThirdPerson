using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private const string ATTACK_ANIMATION_TAG = "Attack";
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
        if (animator.IsInTransition(0) && nextInfo.IsTag(ATTACK_ANIMATION_TAG))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(ATTACK_ANIMATION_TAG))
        {
            return currentInfo.normalizedTime;
        }
        return 0;
    }
}
