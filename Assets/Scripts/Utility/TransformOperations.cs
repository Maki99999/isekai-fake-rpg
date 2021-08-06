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

    public static IEnumerator MoveToLocal(Transform transform, Vector3 newPos, Vector3 newRot, float duration)
    {
        Vector3 oldPos = transform.localPosition;
        Vector3 oldRot = transform.localEulerAngles;

        float rate = 1f / duration;
        float fSmooth;
        for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
        {
            fSmooth = Mathf.SmoothStep(0f, 1f, f);
            transform.localPosition = Vector3.Lerp(oldPos, newPos, fSmooth);
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(oldRot, newRot, fSmooth));

            yield return null;
        }

        transform.localPosition = newPos;
        transform.localRotation = Quaternion.Euler(newRot);
    }

    public static IEnumerator MoveToLocal(Transform transform, Vector3 newPos, float duration)
    {
        Vector3 oldPos = transform.localPosition;

        float rate = 1f / duration;
        float fSmooth;
        for (float f = 0f; f <= 1f; f += rate * Time.deltaTime)
        {
            fSmooth = Mathf.SmoothStep(0f, 1f, f);
            transform.localPosition = Vector3.Lerp(oldPos, newPos, fSmooth);

            yield return null;
        }

        transform.localPosition = newPos;
    }
}
