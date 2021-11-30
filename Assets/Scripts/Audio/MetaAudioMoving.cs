using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MetaAudioMoving : MonoBehaviour
    {
        Transform parent;
        Vector3 offset;

        void Start()
        {
            parent = transform.parent;
            offset = transform.localPosition;
        }

        void Update()
        {
            transform.SetParent(GameController.Instance.meta3dAudio);
            transform.localPosition = parent.position + offset;
        }
    }
}
