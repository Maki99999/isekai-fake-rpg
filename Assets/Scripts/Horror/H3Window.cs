using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H3Window : MonoBehaviour, ISaveDataObject
    {
        public GameObject normalWindow;
        public GameObject brokenWindow;

        public AudioSource audio1;
        public AudioSource audio2;
        public AudioSource cleanUpSound;

        public GameObject[] shards;
        public GameObject cardboard;

        private State state = State.NOT_TRIGGERED;

        public string saveDataId => "H3Window";

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            state = (State)dictEntry.GetInt("triggered", (int)State.NOT_TRIGGERED);
            if (state == State.TRIGGERED)
            {
                normalWindow.SetActive(false);
                brokenWindow.SetActive(true);
            }
            else if (state == State.CLEANED_UP)
            {
                normalWindow.SetActive(false);
                brokenWindow.SetActive(true);

                foreach (GameObject shard in shards)
                    shard.SetActive(false);
                cardboard.SetActive(true);
            }
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("triggered", (int)state);
            return entry;
        }

        public void Trigger()
        {
            if (state != State.NOT_TRIGGERED)
                return;
            state = State.TRIGGERED;

            GameController.Instance.horrorEventManager.StartEvent("H12");

            StartCoroutine(GameController.Instance.playerEventManager.FocusObject(false, transform, 2f));
            normalWindow.SetActive(false);
            brokenWindow.SetActive(true);
            audio1.PlayDelayed(0.25f);
            audio2.PlayDelayed(0.65f);
            StartCoroutine(Dialogue());
        }

        private IEnumerator Dialogue()
        {
            yield return new WaitForSeconds(3f);
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "What the..." });
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
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

            foreach (GameObject shard in shards)
                shard.SetActive(false);
            cardboard.SetActive(true);
            cleanUpSound.Play();
            yield return new WaitForSeconds(2.5f);

            GameController.Instance.fadingAnimator.SetBool("Black", false);
            yield return new WaitForSeconds(0.75f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
        }

        private enum State
        {
            NOT_TRIGGERED,
            TRIGGERED,
            CLEANED_UP
        }
    }
}
