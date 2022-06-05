using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class EntranceDoor : MonoBehaviour, Useable
    {
        public Outline outline;
        public T10Trash t10;
        public AudioSource trashSfx;

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
            StartCoroutine(DisposeOfTrash());
        }

        IEnumerator DisposeOfTrash()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            yield return new WaitForSeconds(0.4f);
            trashSfx.Play();
            yield return new WaitForSeconds(2.8f);

            t10.TrashDisposedOf();
            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(2f);

            GameController.Instance.playerEventManager.FreezePlayers(false);
            gameObject.SetActive(false);
        }
    }
}
