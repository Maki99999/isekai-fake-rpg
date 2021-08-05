using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformOperations
{
    public static IEnumerator MoveTo(Transform transform, Vector3 newPos, Quaternion newRot, float duration)
    {
        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        float rate = 1f / duration;
        float fSmooth;
        for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
        {
            fSmooth = Mathf.SmoothStep(0f, 1f, f);
            transform.position = Vector3.Lerp(oldPos, newPos, fSmooth);
            transform.rotation = Quaternion.Lerp(oldRot, newRot, fSmooth);

            yield return null;
        }

        transform.position = newPos;
        transform.rotation = newRot;
    }

    public static IEnumerator MoveTo(Transform transform, Vector3 newPos, Vector3 newRot, float duration)
    {
        yield return MoveTo(transform, newPos, Quaternion.Euler(newRot), duration);
    }
}
