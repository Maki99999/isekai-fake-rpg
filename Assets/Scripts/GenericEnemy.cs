using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Default
{
    public class GenericEnemy : Entity
    {
        public float attackRange;
        public float detectionRange;

        public int damagePerAttack;
        public float attackOffset;
        public float attackCooldown;

        private Transform target;
        private NavMeshAgent agent;
        private Animator animator;

        private bool inCooldown = false;

        private void Start()
        {
            OnStart();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            OnUpdate();
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= detectionRange)
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
    }
}
