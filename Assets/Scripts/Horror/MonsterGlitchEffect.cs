using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MonsterGlitchEffect : MonoBehaviour
    {
        private const float maxRangeSqrt = 36f;
        private MonsterGlitchEffectReceiver[] receivers;

        void Start()
        {
            receivers = FindObjectsOfType<MonsterGlitchEffectReceiver>();
        }

        void LateUpdate()
        {
            foreach (MonsterGlitchEffectReceiver receiver in receivers)
            {
                Vector3 direction = transform.position - receiver.transform.position;
                float distance = direction.sqrMagnitude;
                bool inRange = distance < maxRangeSqrt;

                float angle = Vector3.Angle(direction, receiver.transform.forward);
                angle = Mathf.Clamp(angle, 0, 60);

                float percent = inRange ? 1 - (angle / 60f) : 0f;
                receiver.desiredPercent = Mathf.Max(percent, receiver.desiredPercent);
            }
        }
    }
}
