using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T13WashHands : MonoBehaviour, Useable, Task
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
            if (!inUse)
            {
                StartCoroutine(WashHandsAnim());
            }
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I want to wash hands first." });
            return true;
        }

        IEnumerator WashHandsAnim()
        {
            inUse = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, true);

            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            audio.Play();

            if (GameController.Instance.storyManager.currentTaskId.Equals("T13"))
            {
                GameController.Instance.storyManager.TaskFinished();
                GameController.Instance.horrorEventManager.StartEvent("H8");
            }

            yield return new WaitForSeconds(1.5f);
            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(1.5f);


            inUse = false;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        public void SkipTask() { }
    }
}
