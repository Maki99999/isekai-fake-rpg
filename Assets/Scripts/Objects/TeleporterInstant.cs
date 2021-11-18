using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class TeleporterInstant : MonoBehaviour
    {
        public Transform newPos;
        public AudioSource audioFx;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                TeleportPlayer();
            }
        }

        public virtual void TeleportPlayer()
        {
            GameController.Instance.playerEventManager.TeleportPlayer(true, newPos);

            GameController.Instance.gameGuiFxAnimator.SetTrigger("Teleporter");
            if (audioFx != null)
                audioFx.Play();
        }
    }
}
