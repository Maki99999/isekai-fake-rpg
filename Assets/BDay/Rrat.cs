using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rrat : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 originalPosition;

    public float wanderCooldown;
    public float wanderRange;
    private Vector3 currentWanderTarget;

    public AudioSource audioSource;

    private void Start()
    {
        originalPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();

        if (wanderCooldown > 0f)
            StartCoroutine(CalculateWanderPositions());

    }

    private void Update()
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

            yield return new WaitForSeconds(Random.Range(wanderCooldown - 5f, wanderCooldown + 5f));
            audioSource.PlayDelayed(2f);
        }
    }
}
