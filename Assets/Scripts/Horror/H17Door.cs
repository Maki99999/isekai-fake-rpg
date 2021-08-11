using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H17Door : MonoBehaviour
    {
        public OpenableDoor door;
        public float chance = 0.05f;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && Random.value < chance)
                door.Close();
        }
    }
}
