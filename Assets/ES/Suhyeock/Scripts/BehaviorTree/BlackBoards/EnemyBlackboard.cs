using UnityEngine;
using UnityEngine.AI;

namespace ES
{
    public class EnemyBlackboard : BaseBlackboard
    {
        public float damage = 10f;
        public float attackRange = 2f;
        public float detectionRange = 10f;
        public float attackSize = 2f;
        public float patrolRadius = 5f;
        public LayerMask LayerMask;
        public Transform playerTransform;

        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public Vector3 targetMovePosition;

        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //characterController = GetComponent<CharacterController>();
            navMeshAgent.speed = speed;
        }
    }
}
