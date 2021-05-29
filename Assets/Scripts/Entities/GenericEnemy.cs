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

        [Space(10)]
        public int xpReward;
        public int coinReward;

        [Space(10)]
        public float wanderCooldown;
        public float wanderRange;
        private Vector3 currentWanderTarget;

        private PlayerController player;
        private NavMeshAgent agent;
        private Animator animator;

        private bool inCooldown = false;
        private bool gotAttacked = false;
        private bool gotAttackedDuringAttack = false;
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

            player = GameController.Instance.gamePlayer;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            if (wanderCooldown > 0f)
                StartCoroutine(CalculateWanderPositions());
        }

        private void Update()
        {
            if (isDead)
                return;

            float distance = Vector3.Distance(player.transform.position, transform.position);
            entityStats.SetZValue(distance);
            if (gotAttacked && distance >= detectionRange * 2)
            {
                gotAttacked = false;
                entityStats.SetHideUi(true);
            }
            else if (gotAttacked || distance <= detectionRange)
            {
                WalkToTarget();

                if (distance <= attackRange)
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
            Vector3 direction = (player.transform.position - transform.position).normalized;
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
                if (isDead)
                    break;
            }
        }

        private IEnumerator IdleSoundAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            fxAudioSource.PlayOneShot(idleFx);
        }

        private void WalkToTarget()
        {
            agent.SetDestination(player.transform.position);
        }

        private IEnumerator AttackTarget()
        {
            inCooldown = true;
            animator.SetTrigger("Attack");
            fxAudioSource.PlayOneShot(attackFx);

            gotAttackedDuringAttack = false;
            yield return new WaitForSeconds(attackOffset * 0.8f);
            if (!gotAttackedDuringAttack)
            {
                yield return new WaitForSeconds(attackOffset * 0.2f);
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= attackRange && !isDead)
                    player.GetComponent<PlayerController>().stats.ChangeHp(-damagePerAttack);
            }

            yield return new WaitForSeconds(attackCooldown);
            inCooldown = false;
        }

        public void ChangedHp(int value)
        {
            if (value < 0 && entityStats.hp > 0)
            {
                gotAttacked = true;
                gotAttackedDuringAttack = true;
                entityStats.SetHideUi(false);
                animator.SetTrigger("Hit");
                fxAudioSource.PlayOneShot(hitFx);
            }
            else if (entityStats.hp <= 0)
                Die();
        }

        private void Die()
        {
            isDead = true;
            entityStats.SetHideUi(false);
            player.stats.ChangeCoins(coinReward);
            player.stats.AddXp(xpReward);

            agent.isStopped = true;
            List<Collider> colliders = new List<Collider>(GetComponents<Collider>());
            colliders.AddRange(GetComponentsInChildren<Collider>());
            foreach (Collider collider in colliders)
                collider.enabled = false;

            animator.SetTrigger("Die");
            fxAudioSource.PlayOneShot(deathFx);
            StartCoroutine(DieEnumerator());
        }

        private IEnumerator DieEnumerator()
        {
            yield return new WaitForSeconds(1f);
            Destroy(entityStats);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));

            Vector3 currPos = transform.localPosition;
            Vector3 currScale = transform.localScale;
            for (float f = 0f; f <= 1f; f += .5f * Time.deltaTime)
            {
                transform.localPosition = Vector3.Lerp(currPos, currPos - Vector3.up, f);
                yield return null;
            }
            for (float f = 0f; f <= 1f; f += Time.deltaTime)
            {
                transform.localScale = Vector3.Lerp(currScale, Vector3.one * 0.0001f, f);
                transform.localPosition = currPos - Vector3.up;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
