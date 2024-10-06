using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader {get; private set;}
    [field: SerializeField] public CharacterController CharacterController {get; private set;}
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public Health Health {get; private set;}
    [field: SerializeField] public float FreeLookMovementSpeed {get; private set;}
    [field: SerializeField] public float TargetingMovementSpeed {get; private set;}
    [field: SerializeField] public float RotationDamping {get; private set;}
    [field: SerializeField] public Targeter Targeter {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public WeaponDamage Weapon {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    public Transform MainCameraTransform { get; private set;}
    [field: SerializeField] public float ImpactDuration {get; private set;} = 1f;

    private void OnEnable()
    {
        Health.OnTakeDamage += OnDamageTaken;
        Health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= OnDamageTaken;
        Health.OnDeath -= HandleDeath;
    }
    private void Start()
    {
        MainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }

    private void OnDamageTaken()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDeath()
    {
        SwitchState(new PlayerDeathState(this));
    }
}
