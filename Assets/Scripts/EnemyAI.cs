using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

//namespace DefaultNamespace
//{
public class EnemyAI : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 6f;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private float _distanceToTarget = Mathf.Infinity;
        private bool _attacking = false;
        private float _attackTime = 2.1f;
        Vector2 velocity = Vector2.zero;
        private Rigidbody rb;
        //private float xMin = -0.5f, xMax = 0.5f;
        Vector2 smoothDeltaPosition = Vector2.zero;
        private bool dead;
        public int powerUpChance;
        public GameObject[] powerUps;
        public int health;
        public BoxCollider attackHitBox;
        public AudioSource sound;
        void Start()
        {
            sound = GetComponent<AudioSource>();
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
    public void Damage()
    {
        health--;
        if(health<= 0)
        {
            Die();
        }

    }
    public void Die()
    {
        if (!dead)
        {
            sound.Play();
            _animator.SetTrigger("die");
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.updatePosition = false;
            _animator.SetBool("move", false);
            dead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            attackHitBox.enabled = false;
            rb.useGravity = false;
            _navMeshAgent.enabled = false;
            var powerup = UnityEngine.Random.Range(0, powerUpChance + 1);
            if (powerup == powerUpChance && gameObject.name.StartsWith("Parasite"))
            {
                Instantiate(powerUps[UnityEngine.Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            }
            if (powerup == powerUpChance && gameObject.name.StartsWith("Level2Parasite"))
            {
                var pu = Instantiate(powerUps[UnityEngine.Random.Range(0, 5)], new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                if (pu.gameObject.name.StartsWith("vial"))
                {
                    pu.transform.Rotate(new Vector3(-77.413f, 0f, 0f));
                }
            }
        }
    }
    public bool getDeadStatus()
    {
        return dead;
    }
}
    
//}