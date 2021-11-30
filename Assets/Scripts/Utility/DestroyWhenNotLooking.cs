using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class DestroyWhenNotLooking : MonoBehaviour
    {
        public float detectionAngle = 80f;

        void Update()
        {
            Vector3 deltaPos = transform.position - GameController.Instance.metaPlayer.transform.position;
            deltaPos = new Vector3(deltaPos.x, 0f, deltaPos.z);
            deltaPos.Normalize();
            float currentAngle = Vector3.Angle(GameController.Instance.metaPlayer.transform.forward, deltaPos);

            if (currentAngle > detectionAngle)
                Destroy(gameObject);
        }
    }
}
