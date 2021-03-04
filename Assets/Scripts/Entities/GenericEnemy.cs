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
        private bool isDead = false;
        private Vector3 originalPosition;

        private EntityStats entityStats;

        [Space(10)]
        public AudioSource fxAudioSource;
        public AudioClip idleFx;
        public AudioClip hitFx;
        public AudioClip attackFx;
        public AudioClip deathFx;

        private void Start()
        {
            entityStats = GetComponent<EntityStats>();
            entityStats.entityStatsObservers.Add(this);

            originalPosition = transform.position;

            target = GameController.Instance.gamePlayer.transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            if (wanderCooldown > 0f)
                StartCoroutine(CalculateWanderPositions());
        }

        private void Update()
        {
            if (isDead)
                return;

            float distance = Vector3.Distance(target.position, transform.position);
            entityStats.SetZValue(distance);
            if (gotAttacked && distance >= detectionRange * 2)
            {
                gotAttacked = false;
                entityStats.SetHideUi(true);
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
            agent.SetDestination(currentWanderTarget);
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

                StartCoroutine(IdleSoundAfter(Random.value * wanderCooldown));
                yield return new WaitForSeconds(wanderCooldown);
            }
        }

        private IEnumerator IdleSoundAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            fxAudioSource.PlayOneShot(idleFx);
        }

        private void WalkToTarget()
        {
            agent.SetDestination(target.position);
        }

        private IEnumerator AttackTarget()
        {
            inCooldown = true;
            animator.SetTrigger("Attack");
            fxAudioSource.PlayOneShot(attackFx);

            yield return new WaitForSeconds(attackOffset);
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= attackRange)
                target.GetComponent<PlayerController>().entityStats.ChangeHp(-damagePerAttack);

            yield return new WaitForSeconds(attackCooldown);
            inCooldown = false;
        }

        public void ChangedHp(int value)
        {
            if (value < 0)
            {
                gotAttacked = true;
                entityStats.SetHideUi(false);
                animator.SetTrigger("Hit");
                fxAudioSource.PlayOneShot(hitFx);
            }

            if (entityStats.hp <= 0)
                Die();
        }

        private void Die()
        {
            animator.SetTrigger("Die");
            agent.isStopped = true;
            fxAudioSource.PlayOneShot(deathFx);
            StartCoroutine(DieEnumerator());
        }

        private IEnumerator DieEnumerator()
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
                yield return null;
            Destroy(gameObject);
        }
    }
}
