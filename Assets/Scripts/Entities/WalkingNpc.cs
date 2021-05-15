using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingNpc : MonoBehaviour
{
    public Animator animator;

    public float stopTimeMin;
    public float stopTimeMax;
    public float speed;

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

            animator.SetBool("Walking", true);
            foreach (Vector3 point in points)
            {
                if ((transform.position - point).sqrMagnitude <= 0.01f)
                    continue;

                transform.rotation = Quaternion.LookRotation(point - transform.position);
                while ((transform.position - point).sqrMagnitude > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
                    yield return null;
                }
            }
            animator.SetBool("Walking", false);
            yield return new WaitForSeconds(Random.Range(stopTimeMin, stopTimeMax));
        }
    }
}
