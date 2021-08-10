using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H20Eyes : MonoBehaviour
    {
        public float maxXRot;
        public float maxYRot;

        float origXRot;
        float origYRot;
        Transform player;

        void Awake()
        {
            origXRot = transform.eulerAngles.x;
            origYRot = transform.eulerAngles.y;
        }

        void Start()
        {
            player = GameController.Instance.metaPlayer.cam.transform;
        }

        void Update()
        {
            Vector3 relativePos = player.position - transform.position;
            Vector3 rotation = Quaternion.LookRotation(relativePos, Vector3.up).eulerAngles;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                                               new Vector3(Mathf.Clamp(rotation.x, origXRot - maxXRot, origXRot + maxXRot),
                                                           Mathf.Clamp(rotation.y, origYRot - maxYRot, origYRot + maxYRot),
                                                           0f),
                                               Time.deltaTime / 2f);
        }
    }
}
