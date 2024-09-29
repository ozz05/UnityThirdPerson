using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private List<Target> _targets = new List<Target>();
    public Target CurrentTarget {get; private set;}
    private void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent<Target>(out Target target)) {return;}
        target.OnTargetDestroyed += RemoveTarget;
        _targets.Add(target);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.TryGetComponent<Target>(out Target target)) {return;}
        RemoveTarget(target);
    }

    private void RemoveTarget(Target target)
    {
        if (target == CurrentTarget)
        {
            
            _cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.OnTargetDestroyed -= RemoveTarget;
        _targets.Remove(target);
    }
    public bool SelectTarget()
    {
        if (_targets.Count <= 0) { return false; }
        CurrentTarget = _targets[0];
        _cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;
        _cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
}
