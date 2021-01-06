using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Default
{
    public class GenericEnemy : MonoBehaviour, EntityStatsObserver
    {
        public float attackRange;
        public float detectionRange;

        public int damagePerAttack;
        public float attackOffset;
        public float attackCooldown;

        public float wanderCooldown;
        public float wanderRange;
        private Vector3 currentWanderTarget;

        private Transform target;
        private NavMeshAgent agent;
        private Animator animator;

        private bool inCooldown = false;
        private bool gotAttacked = false;
        private Vector3 originalPosition;

        private EntityStats entityStats;

        private void Start()
        {
            entityStats = GetComponent<EntityStats>();
            entityStats.entityStatsObservers.Add(this);

            originalPosition = transform.position;

            target = GameController.Instance.player.transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            if (wanderCooldown > 0f)
                StartCoroutine(CalculateWanderPositions());
        }

        private void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (gotAttacked && distance >= detectionRange * 2)
            {
                gotAttacked = false;
            }
            else if (gotAttacked || distance <= detectionRange)
            {
                WalkToTarget();

                if (distance <= agent.stoppingDistance + 0.15f)
                {
                    if (!inCooldown)
                        StartCoroutine(AttackTarget());

                    FaceTarget();
                }
            }
            else
            {
                RoamAimlessly();
            }

            animator.SetFloat("MovingSpeed", (agent.velocity.sqrMagnitude) / (8f));
        }

        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        private void RoamAimlessly()
        {
            agent.SetDestination(currentWanderTarget);   //TODO
        }

        private IEnumerator CalculateWanderPositions()
        {
            while (enabled)
            {
                Vector3 randDirection = Random.insideUnitSphere * wanderRange;
                randDirection += originalPosition;

                NavMeshHit navHit;
                NavMesh.SamplePosition(randDirection, out navHit, wanderRange, -1);

                currentWanderTarget = navHit.position;

                yield return new WaitForSeconds(wanderCooldown);
            }
        }

        private void WalkToTarget()
        {
            agent.SetDestination(target.position);
        }

        private IEnumerator AttackTarget()
        {
            inCooldown = true;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackOffset);
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= attackRange)
                target.GetComponent<PlayerController>().ChangeHp(-damagePerAttack);
            yield return new WaitForSeconds(attackCooldown);
            inCooldown = false;
        }

        public void ChangedHp(int value)
        {
            if (value < 0)
            {
                gotAttacked = true;
                animator.SetTrigger("Hit");
            }

            if (entityStats.hp <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
