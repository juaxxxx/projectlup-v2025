using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

namespace LUP.ES
{
    public enum EnemyState
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        Death = 3,
       
    }

    public class EnemyBlackboard : BaseBlackboard
    {
        public float damage = 10f;
        public float attackRange = 2f;
        public float detectionRange = 10f;
        public float maxRange = 20f;
        public float attackSize = 2f;
        public float patrolRadius = 5f;
        public LayerMask LayerMask;
        [HideInInspector]
        public Transform playerTransform;

        [HideInInspector]
        public EnemyState currentState;

        [HideInInspector]
        public LootSpawner lootSpawner;
        [HideInInspector]
        public bool isDetected = false;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public Vector3 targetMovePosition;
        [HideInInspector]
        public Vector3 initialPosition;
        [HideInInspector]
        public Animator animator;

        [HideInInspector]
        public bool doAttack = false;
        [HideInInspector]
        public bool AttackEnd = false;

        public void Start()
        {
            playerTransform = FindAnyObjectByType<PlayerBlackboard>().transform;
            lootSpawner = GetComponent<LootSpawner>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
            navMeshAgent.acceleration = 16;
            navMeshAgent.angularSpeed = 200;
            initialPosition =  navMeshAgent.transform.position;
            animator = GetComponentInChildren<Animator>();
            currentState = EnemyState.Idle;
        }

        public void ChangeState(EnemyState newState)
        {
            if (animator == null)
                return;
            if (currentState == newState) return;
            currentState = newState;
            animator.SetInteger("StateIndex", (int)currentState);
        }
    }

}
