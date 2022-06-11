using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H4FuseBox : OutlineCreator, Useable
    {
        public Animator animator;
        public Lamp basementLamp;

        public AudioSource audioPower;
        public AudioClip fxPowerOn;
        public AudioClip fxPowerOff;
        public AudioSource audioBox;
        public AudioClip fxBoxOpen;
        public AudioClip fxBoxClose;

        [Space(10)]
        public PhoneHolding phone;

        bool open = false;
        bool powerOff = false;
        bool inAnimation = false;

        protected override void Start()
        {
            base.Start();
        }

        public void PowerOff()
        {
            GameController.Instance.metaHouseController.SetPower(false);
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Has the fuse blown?", "Back to the basement..." });
            powerOff = true;
            animator.SetBool("FuseBlown", true);
            audioPower.clip = fxPowerOff;
            audioPower.Play();
            GameController.Instance.metaPlayer.AddItem(phone, true);
            phone.ActivateFlashlight();
            GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.TOGGLE_LIGHT);
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        void Useable.Use()
        {
            if (inAnimation)
                return;

            if (powerOff)
                StartCoroutine(Fuse());
            else
            {
                open = !open;
                animator.SetBool("Open", open);

                if (open)
                {
                    audioBox.clip = fxBoxOpen;
                    audioBox.PlayDelayed(0.28f);
                }
                else
                {
                    audioBox.clip = fxBoxClose;
                    audioBox.Play();
                }
            }
        }

        IEnumerator Fuse()
        {
            inAnimation = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            StartCoroutine(GameController.Instance.playerEventManager.LookAt(false, transform.position - 0.15f * Vector3.up, 1f));

            if (!open)
            {
                animator.SetBool("Open", true);
                audioBox.clip = fxBoxOpen;
                audioBox.PlayDelayed(0.28f);
                yield return new WaitForSeconds(1.6f);
            }
            animator.SetBool("FuseBlown", false);
            audioPower.clip = fxPowerOn;
            audioPower.Play();
            yield return new WaitForSeconds(0.2f);

            basementLamp.TurnOn();
            GameController.Instance.metaHouseController.SetPower(true);
            phone.playerHasPhone = false;
            phone.DeactivateFlashlight();
            GameController.Instance.metaPlayer.RemoveItem(phone);

            yield return new WaitForSeconds(0.6f);
            animator.SetBool("Open", false);
            audioBox.clip = fxBoxClose;
            audioBox.Play();
            yield return new WaitForSeconds(1.2f);

            open = false;
            powerOff = false;
            inAnimation = false;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            GameController.Instance.horrorEventManager.StartEvent("H14");
        }

        void Useable.LookingAt()
        {
            outlineHelper.ShowOutline();
        }
    }
}
