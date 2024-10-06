using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public CharacterController CharacterController {get; private set;}
    [field: SerializeField] public Health Player {get; private set;}
    [field: SerializeField] public Health Health {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public NavMeshAgent Agent {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}
    [field: SerializeField] public WeaponDamage Weapon {get; private set;}
    [field: SerializeField] public Target Target {get; private set;}

    [field: SerializeField] public float PlayerChaseRange {get; private set;}
    [field: SerializeField] public int MovementSpeed {get; private set;}
    [field: SerializeField] public float AttackingRange {get; private set;} = 1f;
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

    private void OnDamageTaken()
    {
        SwitchState(new EnemyImpactState(this));
    }
    private void HandleDeath()
    {
        SwitchState(new EnemyDeathState(this));
    }
    private void Start() {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.TryGetComponent<Health>(out Health health))
        {
            Player = health;
        }
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);
    }
}
