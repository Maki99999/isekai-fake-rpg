using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T11Dishes : MonoBehaviour, Useable
    {
        public Outline outline;
        private OutlineHelper outlineHelper;
        public GameObject dishes;
        public AudioSource waterSfx;
        public T4WashingMachine t4;

        bool locked = false;

        private void Start()
        {
            outlineHelper = new OutlineHelper(this, outline);

            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "The dishes are next." });
            GameController.Instance.horrorEventManager.StartEvent("H11");
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        public void LookingAt()
        {
            outlineHelper.ShowOutline();
        }

        public void Use()
        {
            if (!locked)
                StartCoroutine(DoDishes());
        }

        IEnumerator DoDishes()
        {
            locked = true;

            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            dishes.SetActive(false);
            waterSfx.Play();
            yield return new WaitForSeconds(2f);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(0.75f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            t4.MachineShouldFinish();
            gameObject.SetActive(false);
        }

        public void SkipTask()
        {
            dishes.SetActive(false);
        }
    }
}
