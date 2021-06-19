using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class OpenableDoor : OutlineCreator, Useable
    {
        public Animator doorAnim;
        public AudioSource doorAudio;

        public bool currentlyOpen = false;

        protected override void Start()
        {
            base.Start();

            if (doorAnim != null)
                doorAnim.SetBool("Open", currentlyOpen);
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            currentlyOpen = !currentlyOpen;

            if (doorAnim != null)
                doorAnim.SetBool("Open", currentlyOpen);
            if (doorAudio != null)
                doorAudio.Play();
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
