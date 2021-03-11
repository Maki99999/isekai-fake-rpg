using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingNpc : MonoBehaviour
{
    public NavMeshAgent agent;
    public float stopTimeMin;
    public float stopTimeMax;

    WalkingNpcArea area;

    void Start()
    {
        area = GetComponentInParent<WalkingNpcArea>();
        StartCoroutine(Walk());
    }

    IEnumerator Walk()
    {
        while (enabled)
        {
            Vector3[] points = area.GetNextWalkPath(transform.position);
            DrawDebugLines(points);

            foreach (Vector3 point in points)
            {
                agent.SetDestination(point);
                yield return null;
                yield return new WaitUntil(() => (agent.remainingDistance != Mathf.Infinity
                        && agent.pathStatus == NavMeshPathStatus.PathComplete
                        && agent.remainingDistance == 0));
            }
            yield return new WaitForSeconds(Random.Range(stopTimeMin, stopTimeMax));
        }
    }

    void DrawDebugLines(Vector3[] points)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Debug.DrawLine(Vector3.up + points[i], Vector3.up + points[i + 1], Color.cyan, 10);
        }
    }
}
