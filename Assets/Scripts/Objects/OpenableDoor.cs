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

        public bool locked = false;

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
            if (locked)
            {
                doorAnim.SetTrigger("Rattle");
                doorAudio.clip = sfxRattle;
                doorAudio.Play();
                return;
            }
            else if (currentlyOpen)
                Close();
            else
                Open();
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
    }
}
