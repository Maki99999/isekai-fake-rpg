using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class TeleporterInstant : MonoBehaviour
    {
        public Transform newPos;
        public AudioSource audioFx;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameController.Instance.eventManager.TeleportPlayer(true, newPos);

                GameController.Instance.gameGuiFxAnimator.SetTrigger("Teleporter");
                if (audioFx != null)
                    audioFx.Play();
            }
        }
    }
}
