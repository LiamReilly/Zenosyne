using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//namespace DefaultNamespace
//{
public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 6f;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private float _distanceToTarget = Mathf.Infinity;
        private bool _attacking = false;
        private float _attackTime = 2.1f;
        Vector2 velocity = Vector2.zero;
        private Rigidbody rb;
        private float xMin = -0.5f, xMax = 0.5f;
        Vector2 smoothDeltaPosition = Vector2.zero;
        private bool dead;
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            //_navMeshAgent.updatePosition = false;
        }

        void Update()
        {
            _distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (!dead)
            {
                if (_distanceToTarget <= chaseRange && _distanceToTarget > attackRange)
                {
                    _animator.SetBool("move", true);
                    chaseTarget();
                }
                else
                {
                    _animator.SetBool("move", false);
                }

                if (_distanceToTarget <= attackRange)
                {
                    if (!_attacking)
                    {
                         attackTarget();
                         _attacking = true;
                    }

            }
        }
            

        }

        void chaseTarget()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(target.position);
            //_navMeshAgent.speed = 3.5f;
        }

        void attackTarget()
        {
            _animator.Play("Attack",0,0f);
            StartCoroutine(WaitForAttack(_attackTime));
            
        }
    IEnumerator WaitForAttack(float f)
    {
        yield return new WaitForSeconds(f);
            _attacking = false;
    }
    public void Die()
    {
        if (!dead)
        {
            _animator.SetTrigger("die");
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.updatePosition = false;
            _animator.SetBool("move", false);
            dead = true;
        }

    }
}
    
//}