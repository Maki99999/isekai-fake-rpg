using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class LightSwitch : OutlineCreator, Useable
    {
        public Animator lightSwitchAnim;
        public AudioSource lightSwitchAudio;

        public bool currentlyOn = false;
        public Lamp[] lamps;

        protected override void Start()
        {
            base.Start();

            if (lightSwitchAnim != null)
                lightSwitchAnim.SetBool("On", currentlyOn);
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Useable.Use()
        {
            currentlyOn = !currentlyOn;

            foreach (Lamp lamp in lamps)
                lamp.Toggle();

            if (lightSwitchAnim != null)
                lightSwitchAnim.SetBool("On", currentlyOn);
            if (lightSwitchAudio != null)
                lightSwitchAudio.Play();
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
