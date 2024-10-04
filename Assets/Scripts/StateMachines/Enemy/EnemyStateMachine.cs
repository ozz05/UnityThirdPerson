using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public CharacterController CharacterController {get; private set;}
    [field: SerializeField] public GameObject Player {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public NavMeshAgent NavMeshAgent {get; private set;}
    [field: SerializeField] public Attack[] Attacks {get; private set;}

    [field: SerializeField] public float PlayerChaseRange {get; private set;}
    [field: SerializeField] public int MovementSpeed {get; private set;}
    [field: SerializeField] public float AttackingRange {get; private set;} = 1f;

    private void Start() {

        Player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent.updatePosition = false;
        NavMeshAgent.updateRotation = false;
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);
    }
}
