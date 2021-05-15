using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MetaAudio : MonoBehaviour
    {
        void Start()
        {
            Vector3 currentPos = transform.position;
            transform.SetParent(GameController.Instance.meta3dAudio);
            transform.localPosition = currentPos;
        }
    }
}
