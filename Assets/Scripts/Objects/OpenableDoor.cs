using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class OpenableDoor : OutlineCreator, Useable
    {
        public Animator doorAnim;
        public AudioSource doorAudio;
        public AudioClip sfxOpen;
        public AudioClip sfxClose;
        public AudioClip sfxRattle;

        public bool currentlyOpen = false;
        public bool openFurther = false;

        public State lockedMode = State.UNLOCKED;
        public List<string> notOpeningText;

        protected override void Start()
        {
            base.Start();

            if (doorAnim != null)
            {
                doorAnim.SetBool("Wide", openFurther);
                doorAnim.SetBool("Open", currentlyOpen);
            }
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            if (lockedMode != State.UNLOCKED)
            {
                StartCoroutine(LockedDialogue(lockedMode));
                return;
            }
            else if (currentlyOpen)
                Close();
            else
                Open();
        }

        private IEnumerator LockedDialogue(State lockedMode)
        {
            if (lockedMode == State.LOCKED)
            {
                doorAnim.SetTrigger("Rattle");
                doorAudio.clip = sfxRattle;
                doorAudio.Play();
            }
            else if (lockedMode == State.UNINTERESTING)
            {
                doorAnim.SetBool("Peek", true);
                doorAudio.clip = sfxOpen;
                doorAudio.Play();
            }

            if (notOpeningText.Count > 0)
            {
                GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
                yield return GameController.Instance.dialogue.StartDialogue(notOpeningText);
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
            }

            if (lockedMode == State.UNINTERESTING)
            {
                doorAnim.SetBool("Peek", false);
                doorAudio.clip = sfxClose;
                doorAudio.PlayDelayed(0.2f);
            }
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }

        public void Open()
        {
            if (currentlyOpen)
                return;
            currentlyOpen = true;
            if (doorAnim != null)
                doorAnim.SetBool("Open", true);

            doorAudio.clip = sfxOpen;
            doorAudio.Play();
        }

        public void Close()
        {
            if (!currentlyOpen)
                return;
            currentlyOpen = false;
            if (doorAnim != null)
                doorAnim.SetBool("Open", false);

            doorAudio.clip = sfxClose;
            doorAudio.PlayDelayed(0.5f);
        }

        [System.Serializable]
        public enum State
        {
            UNLOCKED,
            LOCKED,
            UNINTERESTING
        }
    }
}
