using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MonsterCameraEffect : MonoBehaviour
    {
        private Camera[] cameras;

        void Start()
        {
            cameras = FindObjectsOfType<Camera>();
        }

        void Update()
        {

        }
    }
}
