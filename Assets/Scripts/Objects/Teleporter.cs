using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Teleporter : MonoBehaviour
    {
        public Transform newPos;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<PlayerController>().TeleportPlayer(newPos);
        }
    }
}
