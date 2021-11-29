using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ResetTransform : MonoBehaviour
    {
        IEnumerator Start()
        {
            while (isActiveAndEnabled)
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                transform.localScale = Vector3.one;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}