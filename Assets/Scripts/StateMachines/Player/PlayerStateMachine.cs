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
    [field: SerializeField] public float JumpHeight {get; private set;}
    [field: SerializeField] public float JumpSpeed {get; private set;}
    [field: SerializeField] public float ImpactDuration {get; private set;} = 1f;
    [field: SerializeField] public float DodgeDuration {get; private set;}
    [field: SerializeField] public float DodgeDistance {get; private set;}
    [field: SerializeField] public float DodgeCooldownTime {get; private set;} = 2f;
    [field: SerializeField] public float PreviousDodgeTime {get; private set;}

    private void OnEnable()
    {
        PreviousDodgeTime = Time.time - DodgeCooldownTime;
        Health.OnTakeDamage += OnDamageTaken;
        Health.OnDeath += HandleDeath;
    }
    public void SetDodgeTime (float time)
    {
        PreviousDodgeTime = time;
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
