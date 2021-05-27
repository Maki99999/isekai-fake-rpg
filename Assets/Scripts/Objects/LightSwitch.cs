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
        public Light[] lights;

        protected override void Start()
        {
            base.Start();

            foreach (Light light in lights)
                light.enabled = currentlyOn;

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

            foreach (Light light in lights)
                light.enabled = currentlyOn;

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
