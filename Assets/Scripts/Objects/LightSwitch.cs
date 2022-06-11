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
        public Lamp masterLamp;

        protected override void Start()
        {
            base.Start();

            if (lightSwitchAnim != null)
                lightSwitchAnim.SetBool("On", currentlyOn);
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        void Useable.Use()
        {
            currentlyOn = !currentlyOn;

            masterLamp.Toggle();

            if (lightSwitchAnim != null)
                lightSwitchAnim.SetBool("On", currentlyOn);
            if (lightSwitchAudio != null)
                lightSwitchAudio.Play();
        }

        void Useable.LookingAt()
        {
            outlineHelper.ShowOutline();
        }
    }
}
