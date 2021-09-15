using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class WashingDryerMachine : MonoBehaviour, Useable
    {
        public Outline outline;
        public Animator animator;
        public GameObject inside;
        public GameObject basket;
        public GameObject basketClothing;
        public ItemHoldablePickup basketItem;
        public T4WashingMachine t4;
        public bool isDryer;
        public AudioSource rumblingSFX;
        public AudioSource readySFX;
        public AudioSource doorSFX;

        private bool open = false;
        private bool turnedOn = false;
        private bool finished = false;
        private bool beeping = false;

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
            if (!turnedOn)
                StartCoroutine(PutClothesIn());
            else
            {
                if (finished)
                    StartCoroutine(TakeClothesOut());
                else
                    GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "The machine is not done yet." });
            }
        }

        public void Finish()
        {
            animator.SetTrigger("Finished");
            finished = true;
            rumblingSFX.Stop();
            StartCoroutine(Beep());
        }

        IEnumerator Beep()
        {
            beeping = true;
            while (beeping)
            {
                readySFX.Play();
                yield return new WaitForSeconds(10f);
            }
        }

        IEnumerator PutClothesIn()
        {
            if (!open)
            {
                open = true;
                animator.SetBool("Open", true);
                doorSFX.Play();
                yield return new WaitForSeconds(1f);
            }

            if (!GameController.Instance.metaPlayer.HasItem("Laundry Basket"))
            {
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "The laundry basket is upstairs in the bathroom." });
            }
            else
            {
                GameController.Instance.playerEventManager.FreezePlayers(true);
                GameController.Instance.fadingAnimator.SetBool("Black", true);
                yield return new WaitForSeconds(2f);

                inside.SetActive(true);
                turnedOn = true;
                basket.SetActive(true);
                basketClothing.SetActive(false);
                ItemHoldable basketItem = GameController.Instance.metaPlayer.GetItem("Laundry Basket");
                GameController.Instance.metaPlayer.RemoveItem(basketItem);
                Destroy(basketItem.gameObject);

                GameController.Instance.fadingAnimator.SetBool("Black", false);
                yield return new WaitForSeconds(2f);

                open = false;
                animator.SetBool("Open", false);
                doorSFX.Play();
                GameController.Instance.playerEventManager.FreezePlayers(false);
                t4.MachineStarted();

                if (!isDryer)
                    GameController.Instance.storyManager.StartTask("T10", true);

                yield return new WaitForSeconds(1f);
                rumblingSFX.Play();
            }
        }

        IEnumerator TakeClothesOut()
        {
            finished = false;
            open = true;
            animator.SetBool("Open", true);
            doorSFX.Play();
            yield return new WaitForSeconds(1f);

            GameController.Instance.playerEventManager.FreezePlayers(true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(2f);

            inside.SetActive(false);
            basketClothing.SetActive(true);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(2f);

            if (!isDryer)
                basketItem.PickThisUp();
            GameController.Instance.playerEventManager.FreezePlayers(false);
            t4.MachineEmptied();
        }
    }
}
