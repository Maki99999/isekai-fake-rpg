using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Sink : MonoBehaviour, Useable
    {
        public Outline outline;
        new public AudioSource audio;
        private bool inUse = false;

        void Update()
        {
            outline.enabled = false;
        }

        public void LookingAt()
        {
            outline.enabled = true;
        }

        public void Use()
        {
            if (!inUse && isActiveAndEnabled)
            {
                StartCoroutine(WashHandsAnim());
            }
        }

        public IEnumerator WashHandsAnim(bool withH8 = false)
        {
            inUse = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);

            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            audio.Play();

            if (withH8)
            {
                GameController.Instance.horrorEventManager.StartEvent("H8");
            }

            yield return new WaitForSeconds(1.5f);
            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(1.5f);

            inUse = false;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }
    }
}
