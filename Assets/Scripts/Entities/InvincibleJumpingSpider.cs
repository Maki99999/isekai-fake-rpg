using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Default
{
    public class InvincibleJumpingSpider : MonoBehaviour
    {
        public float attackRange;

        public int damagePerAttack;
        public float attackOffset;
        public float attackCooldown;

        [Space(10)]
        public float wanderCooldown;
        public float wanderRange;
        private Vector3 currentWanderTarget;

        private PlayerController player;
        private PlayerController playerM;
        private NavMeshAgent agent;
        private Animator animator;

        private bool inCooldown = false;
        private Vector3 originalPosition;

        private int isEvading = 0;
        private bool isJumping = false;

        [Space(10)]
        public AudioSource fxAudioSource;
        public AudioClip idleFx;
        public AudioClip attackFx;

        [Space(10)]
        public Transform spiderDouble;
        public Animator doubleAnimator;
        public Transform ScreenPos;
        public PcController pcController;

        private void Start()
        {
            originalPosition = transform.position;

            player = GameController.Instance.gamePlayer;
            playerM = GameController.Instance.metaPlayer;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            if (wanderCooldown > 0f)
                StartCoroutine(CalculateWanderPositions());
        }

        private void Update()
        {
            if (isJumping)
                return;

            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= attackRange)
            {
                if (!inCooldown)
                    StartCoroutine(AttackTarget());

                FaceTarget();
            }
            else
            {
                RoamAimlessly();
            }

            animator.SetFloat("MovingSpeed", (agent.velocity.sqrMagnitude) / (8f));
        }

        private void LateUpdate()
        {
            Vector3 locPos = player.cam.transform.InverseTransformPoint(transform.position);
            spiderDouble.position = playerM.cam.transform.TransformPoint(locPos);
            Quaternion locRot = Quaternion.Inverse(player.cam.transform.rotation) * transform.rotation;
            spiderDouble.rotation = playerM.cam.transform.rotation * locRot;
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
            yield return null;
            while (enabled && !isJumping)
            {
                if (isEvading > 0)
                    yield return new WaitForSeconds(1f);
                else
                {
                    Vector3 randDirection = (Vector3.one - Random.insideUnitSphere * 0.2f) * wanderRange;
                    randDirection += originalPosition;

                    NavMeshHit navHit;
                    NavMesh.SamplePosition(randDirection, out navHit, wanderRange + 1f, -1);

                    currentWanderTarget = navHit.position;

                    StartCoroutine(IdleSoundAfter(Random.value * wanderCooldown));
                    yield return new WaitForSeconds(wanderCooldown);
                }
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

            yield return new WaitForSeconds(attackOffset * 1f);
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= attackRange)
                player.GetComponent<PlayerController>().stats.ChangeHp(-damagePerAttack);

            yield return new WaitForSeconds(attackCooldown);
            inCooldown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                StartCoroutine(Evade());
            }
        }

        IEnumerator Evade()
        {
            isEvading++;

            Debug.DrawRay(originalPosition, Vector3.up * 2f, Color.red, 2f);
            Debug.DrawRay(transform.position, Vector3.up * 2f, Color.black, 2f);
            Vector3 newDirection = originalPosition - transform.position;
            newDirection += originalPosition;
            Debug.DrawRay(newDirection, Vector3.up * 2f, Color.blue, 2f);

            NavMeshHit navHit;
            NavMesh.SamplePosition(newDirection, out navHit, wanderRange + 1f, -1);

            currentWanderTarget = navHit.position;

            float oldAccel = agent.acceleration;
            agent.acceleration *= 5;

            yield return new WaitForSeconds(2f);
            isEvading--;
            if (isEvading == 0)
                agent.acceleration = oldAccel;
        }

        public void StartJump()
        {
            if (!isJumping)
                StartCoroutine(Jump());
        }

        IEnumerator Jump()
        {
            spiderDouble.gameObject.SetActive(true);
            spiderDouble.SetParent(null);
            isJumping = true;
            inCooldown = true;
            animator.SetTrigger("Attack");
            doubleAnimator.SetTrigger("Attack");
            fxAudioSource.PlayOneShot(attackFx);

            GameController.Instance.playerEventManager.FreezePlayer(true, true, true);
            StartCoroutine(GameController.Instance.playerEventManager.LookAt(true, transform.position - 0.15f * Vector3.up, 1f));
            float startTime = Time.time;
            while (Time.time < startTime + 0.833f)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                yield return null;
            }
            agent.enabled = false;

            Vector3 oldPos = transform.position;
            float rate = 1f / 0.25f;
            float fSmooth;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.Pow(f, 2);
                pcController.ImmersedValue = 1 - fSmooth;
                transform.localPosition = Vector3.Lerp(oldPos,
                    player.cam.transform.position - 0.12f * player.cam.transform.up - 0.05f * player.cam.transform.forward,
                    fSmooth);

                yield return null;
            }
            oldPos = transform.position;
            Vector3 newPos = player.cam.transform.position - player.cam.transform.forward * 0.5f;
            rate = 1f / 0.2f;
            for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
            {
                fSmooth = Mathf.Pow(f, 2);
                transform.localPosition = Vector3.Lerp(oldPos, newPos, fSmooth);

                yield return null;
            }

            pcController.StartCoroutine(pcController.Immerse(false, 0.6f));
            spiderDouble.gameObject.SetActive(false);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "...", "Felt like it jumped right through the screen." });

            GameController.Instance.playerEventManager.FreezePlayer(true, false);
            gameObject.SetActive(false);
        }
    }
}
