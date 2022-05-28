using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class EntranceDoor : MonoBehaviour, Useable
    {
        public Outline outline;
        public T10Trash t10;

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
            StartCoroutine(Pickup());
        }

        IEnumerator Pickup()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            t10.TrashDisposedOf();

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(2f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            gameObject.SetActive(false);
        }
    }
}
