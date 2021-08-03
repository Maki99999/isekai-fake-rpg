using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    // H4
    public class FuseBox : OutlineCreator, Useable
    {
        public Animator animator;
        public Lamp[] basementLamps;

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
            powerOff = true;
            animator.SetBool("FuseBlown", true);
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
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
            }
        }

        IEnumerator Fuse()
        {
            inAnimation = true;

            animator.SetBool("Open", true);
            yield return new WaitForSeconds(1.25f);
            animator.SetBool("FuseBlown", false);
            yield return new WaitForSeconds(0.2f);

            foreach (Lamp lamp in basementLamps)
                lamp.TurnOn();
            GameController.Instance.metaHouseController.SetPower(true);

            yield return new WaitForSeconds(0.3f);
            animator.SetBool("Open", false);
            yield return new WaitForSeconds(1.2f);
            
            powerOff = false;
            inAnimation = false;
        }

        void Useable.LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
