using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ResetTransform : MonoBehaviour
    {
        void Start()
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }
    }
}