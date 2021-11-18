using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Default
{
    public class EndingEnemy : MonoBehaviour
    {
        public float attackRange;
        public float attackOffset;
        public float attackCooldown;

        public Ending ending;
        public bool part2 = false; 

        [Space(10)]
        private PlayerController player;
        private NavMeshAgent agent;
        private Animator animator;

        private bool inCooldown = false;

        [Space(10)]
        public AudioSource fxAudioSource;
        public AudioClip idleFx;
        public AudioClip attackFx;

        private void Start()
        {
            player = GameController.Instance.metaPlayer;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            WalkToTarget();

            if (distance <= attackRange)
            {
                if (!part2 && !inCooldown)
                    StartCoroutine(AttackTarget());

                FaceTarget();
            }


            animator.SetFloat("MovingSpeed", (agent.velocity.sqrMagnitude) / (8f));
        }

        private void FaceTarget()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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

        public IEnumerator AttackTarget()
        {
            inCooldown = true;
            animator.SetTrigger("Attack");
            fxAudioSource.PlayOneShot(attackFx);

            yield return new WaitForSeconds(attackOffset);
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= attackRange)
                ending.ContinueEnding();

            yield return new WaitForSeconds(attackCooldown);
            inCooldown = false;
        }
    }
}
