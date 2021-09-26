using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class KnifeItem : ItemHoldable
    {
        public Animator animator;
        public AudioSource slashAudio;
        public AudioClip[] slashAudioClips;

        private bool isCharging = false;

        public override MoveData UseItem(MoveData inputData)
        {
            bool isPressing = inputData.axisPrimary > 0;

            if (isPressing)
            {
                isCharging = true;
                animator.SetBool("Charging", true);
            }
            else if (!isPressing && isCharging)
            {
                isCharging = false;
                Swing();
            }

            return inputData;
        }

        private void Swing()
        {
            animator.SetBool("Charging", false);
            slashAudio.clip = slashAudioClips[Random.Range(0, slashAudioClips.Length)];
            slashAudio.Play();
        }

        public override void OnUnequip()
        {
            animator.SetBool("Show", false);
            animator.SetTrigger("Hide");
        }

        public override void OnEquip()
        {
            animator.SetBool("Show", true);
        }

        public override void OnPickup()
        {
            GameController.Instance.storyManager.TaskFinished();
            animator.enabled = true;
        }
    }
}
