using UnityEngine;
using UnityEngine.AI;

namespace LUP.ES
{
    public class EnemyBlackboard : BaseBlackboard
    {
        public float damage = 10f;
        public float attackRange = 2f;
        public float detectionRange = 10f;
        public float maxRange = 20f;
        public float attackSize = 2f;
        public float patrolRadius = 5f;
        public LayerMask LayerMask;
        public Transform playerTransform;

        [HideInInspector]
        public bool isDetected = false;
        [HideInInspector]
        public NavMeshAgent navMeshAgent;
        [HideInInspector]
        public Vector3 targetMovePosition;
        [HideInInspector]
        public Vector3 initialPosition;

        public void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.speed = speed;
            navMeshAgent.acceleration = 16;
            navMeshAgent.angularSpeed = 200;
            initialPosition =  navMeshAgent.transform.position;
        }

    }
}
