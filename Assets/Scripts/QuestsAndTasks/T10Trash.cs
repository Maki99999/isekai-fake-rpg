using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T10Trash : MonoBehaviour, Useable
    {
        public Outline[] outlines;
        private OutlineHelper outlineHelper;
        public Collider useableCollider;
        public ItemHoldable trashItem;
        public GameObject entrance;
        public T4WashingMachine t4;
        public AudioSource trashSfx;

        private bool locked;

        private IEnumerator Start()
        {
            outlineHelper = new OutlineHelper(this, outlines);

            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I still have trash in the kitchen to bring outside." });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);

            yield return new WaitForSeconds(16f);
            GameController.Instance.horrorEventManager.StartEvent("H1");
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        public void LookingAt()
        {
            if (!locked)
                outlineHelper.ShowOutline();
        }

        public void Use()
        {
            if (!locked)
                StartCoroutine(Pickup());
        }

        IEnumerator Pickup()
        {
            locked = true;
            outlineHelper.DestroyOutlines();

            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            trashItem.gameObject.SetActive(true);
            trashSfx.Play();
            GameController.Instance.metaPlayer.AddItem(trashItem, true);
            yield return new WaitForSeconds(2f);

            entrance.SetActive(true);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(2f);
        }

        public void TrashDisposedOf()
        {
            GameController.Instance.metaPlayer.RemoveItem(trashItem);
            trashItem.gameObject.SetActive(false);
            t4.MachineShouldFinish();
            gameObject.SetActive(false);
        }

        public void SkipTask() { }
    }
}
