using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private List<Target> _targets = new List<Target>();
    private Camera _mainCamera;
    public Target CurrentTarget {get; private set;}
    private void Start() {
        _mainCamera = Camera.main;
    }
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
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        foreach(Target target in _targets)
        {
            Vector2 viewPos = _mainCamera.WorldToViewportPoint(target.transform.position);
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }
        
        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
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
