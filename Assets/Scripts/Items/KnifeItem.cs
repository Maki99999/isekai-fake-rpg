using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class KnifeItem : ItemHoldable, ISaveDataObject
    {
        public Animator animator;
        public AudioSource slashAudio;
        public AudioClip[] slashAudioClips;

        private bool equipped = false;
        private bool isCharging = false;

        public string saveDataId => "knifeItem";

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
                animator.SetBool("Charging", false);
            }

            return inputData;
        }

        private void SwingSound()
        {
            slashAudio.clip = slashAudioClips[Random.Range(0, slashAudioClips.Length)];
            slashAudio.Play();
        }

        public override void OnUnequip()
        {
            equipped = false;
            if (isActiveAndEnabled)
            {
                animator.SetBool("Show", false);
                animator.SetTrigger("Hide");
            }
        }

        public override void OnEquip()
        {
            equipped = true;
            animator.SetBool("Show", true);
        }

        public override void OnPickup()
        {
            GameController.Instance.storyManager.TaskFinished("T12");
            GameController.Instance.horrorEventManager.StartEvent("H6");
            animator.enabled = true;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("enabled", enabled ? "true" : "false");
            entry.Add("equipped", equipped ? "true" : "false");
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            if (dictEntry.GetString("enabled", "true") == "false")
            {
                gameObject.SetActive(false);
            }
            else if (dictEntry.GetString("equipped", "false") == "true")
            {
                GameController.Instance.metaPlayer.AddItem(this, true, false);
            }
        }
    }
}
