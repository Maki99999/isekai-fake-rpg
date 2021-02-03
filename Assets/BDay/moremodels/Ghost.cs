using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float height = 1f;
    public float speedMult = 1f;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(UpDown());
    }

    IEnumerator UpDown()
    {
        yield return new WaitForSeconds(Random.Range(0f, 5f));
        while (enabled)
        {
            for (int i = 0; i <= 100; i+= Mathf.RoundToInt(speedMult))
            {
                transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * height, Mathf.SmoothStep(0, 1, i / 100f));
                yield return new WaitForSeconds(.05f);
            }
            for (int i = 0; i <= 100; i+= Mathf.RoundToInt(speedMult))
            {
                transform.position = Vector3.Lerp(startPos + Vector3.up * height, startPos, Mathf.SmoothStep(0, 1, i / 100f));
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
