using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class TeleporterSlow : TeleporterInstant
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
        }

        protected override void TeleportPlayer()
        {
            StartCoroutine(TeleportEvent());
        }

        private IEnumerator TeleportEvent() {
            GameController.Instance.playerEventManager.FreezePlayer(true, true);
            if (audioFx != null)
                audioFx.Play();

            GameController.Instance.gameGuiFxAnimator.SetTrigger("Cave");
            yield return new WaitForSeconds(0.5f);
            
            GameController.Instance.playerEventManager.TeleportPlayer(true, newPos);
            GameController.Instance.playerEventManager.FreezePlayer(true, false);
        }
    }
}
