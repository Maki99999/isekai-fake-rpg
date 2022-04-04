using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H11Glass : MonoBehaviour, ISaveDataObject
    {
        public Animator animator;
        public AudioSource audioSource;

        private bool triggered = false;

        public string saveDataId => "H11Glass";

        public void Triggered()
        {
            triggered = true;
            animator.SetTrigger("Fall");
        }

        public void PlayAudio()
        {
            if (audioSource.isActiveAndEnabled)
                audioSource.Play();
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("triggered", triggered);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;
            if (dictEntry.GetBool("triggered", false))
            {
                audioSource.enabled = false;
                Triggered();
            }
        }
    }
}
