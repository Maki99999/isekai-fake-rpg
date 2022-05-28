using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H11Glass : MonoBehaviour, ISaveDataObject
    {
        public Animator animator;
        public AudioSource clashSound;
        public AudioSource cleanUpSound;

        public GameObject shards;

        private State state = State.NOT_TRIGGERED;

        public string saveDataId => "H11Glass";

        public void Triggered()
        {
            state = State.TRIGGERED;
            animator.SetTrigger("Fall");
        }

        public void PlayAudio()
        {
            if (clashSound.isActiveAndEnabled)
                clashSound.Play();
        }

        public void StartCleanUp()
        {
            StartCoroutine(CleanUp());
        }

        private IEnumerator CleanUp()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            GameController.Instance.fadingAnimator.SetBool("Black", true);
            yield return new WaitForSeconds(1.5f);

            animator.enabled = false;
            shards.SetActive(false);
            cleanUpSound.Play();
            yield return new WaitForSeconds(2.5f);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(0.75f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
            gameObject.SetActive(false);
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("triggered", (int)state);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            state = (State)dictEntry.GetInt("triggered", (int)State.NOT_TRIGGERED);
            if (state == State.TRIGGERED)
            {
                clashSound.enabled = false;
                Triggered();
            }
            else if (state == State.CLEANED_UP)
            {
                gameObject.SetActive(false);
            }
        }

        private enum State
        {
            NOT_TRIGGERED,
            TRIGGERED,
            CLEANED_UP
        }
    }
}
