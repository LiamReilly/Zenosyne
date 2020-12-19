using System;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 6f;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private float _distanceToTarget = Mathf.Infinity;
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            _distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (_distanceToTarget <= chaseRange)
            {
                chaseTarget();
            }

            if (_distanceToTarget <= attackRange)
            {
                attackTarget();
            }

        }

        void chaseTarget()
        {
            _navMeshAgent.isStopped = false; 
            _navMeshAgent.SetDestination(target.position);
        }

        void attackTarget()
        {
            _animator.Play("Attack",0,0f);
            
        }
    }
}