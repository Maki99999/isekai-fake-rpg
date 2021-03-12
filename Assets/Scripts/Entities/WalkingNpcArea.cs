using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNpcArea : MonoBehaviour
{
    public Vector3 centerOffset;
    public float walkPathLength;
    public float walkPathWidth;
    public float standWidth;

    [Space(10)]
    public int prefabCount = 10;
    public GameObject[] npcPrefabs;

    void Start()
    {
        for (int i = 0; i < prefabCount; i++)
        {
            Instantiate(npcPrefabs[Random.Range(0, npcPrefabs.Length)], transform.TransformPoint(
                    centerOffset + Random.Range(-1f, 1f) * walkPathWidth * Vector3.right +
                    Random.Range(-1f, 1f) * walkPathLength * Vector3.forward), Quaternion.Euler(0, 0, 0), transform);
        }
    }

    public Vector3[] GetNextWalkPath(Vector3 currentPos)
    {
        Vector3 currPosLocal = transform.InverseTransformPoint(currentPos);
        Vector3 newStandPos = centerOffset + new Vector3((walkPathWidth + (standWidth * Random.value)) * (Random.value > 0.5f ? 1f : -1f), 0f, Random.Range(-1f, 1f) * walkPathLength);

        float walkX = Random.value * -standWidth * 2f;
        Vector3 walkPos1 = new Vector3(Mathf.Sign(newStandPos.x) * (walkPathWidth + walkX), 0f, currPosLocal.z);
        Vector3 walkPos2 = new Vector3(Mathf.Sign(newStandPos.x) * (walkPathWidth + walkX), 0f, newStandPos.z);

        newStandPos = transform.TransformPoint(newStandPos);
        walkPos1 = transform.TransformPoint(walkPos1);
        walkPos2 = transform.TransformPoint(walkPos2);

        Vector3[] points = new Vector3[] { currentPos, walkPos1, walkPos2, newStandPos };
        //return points;
        return MakeSmoothCurve(points, 4);
    }

    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, int smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1) smoothness = 1;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * smoothness) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }
}
