using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class DialogueManager : MonoBehaviour
    {
        public Animator anim;
        public Text text;
        public GameObject audioSourceOriginal;
        public int audioSourcesCount = 25;
        [Space(10)]
        public AudioClip[] audioClipsNormal;
        public AudioClip[] audioClipsCreepy;
        private AudioClip[] currentAudioClips;

        private int every4thLetter = -1;
        private int currentAudioSource = -1;
        private List<AudioSource> audioSources;

        private bool isPressing = false;
        public bool isInDialogue { get; private set; } = false;

        void Start()
        {
            audioSources = new List<AudioSource>();
            audioSources.Add(audioSourceOriginal.GetComponent<AudioSource>());
            for (int i = 0; i < audioSourcesCount - 1; i++)
                audioSources.Add(Instantiate(audioSourceOriginal, audioSourceOriginal.transform.position,
                    audioSourceOriginal.transform.rotation, transform).GetComponent<AudioSource>());
        }

        public void StartDialogueWithFreeze(List<string> texts)
        {
            StartCoroutine(DialogueWithFreeze(texts));
        }

        IEnumerator DialogueWithFreeze(List<string> texts)
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            yield return new WaitForSeconds(.2f);
            yield return StartDialogue(texts);
            yield return new WaitForSeconds(.2f);
            GameController.Instance.playerEventManager.FreezePlayers(false);
        }

        public IEnumerator StartDialogue(List<string> texts, bool creepy = false)
        {
            if (!isInDialogue)
            {
                isInDialogue = true;
                Reset();
                currentAudioClips = GetRightAudioClips(creepy);
                anim.SetBool("Activated", true);
                yield return new WaitForSeconds(.5f);

                foreach (string sentence in texts)
                {

                    if (PauseManager.isPaused().Value)
                        yield return new WaitWhile(() => PauseManager.isPaused().Value);

                    yield return TypeSentence(sentence);

                    yield return new WaitUntil(() => (!PauseManager.isPaused().Value && IsPressingConfirm()));
                    yield return new WaitUntil(() => (!PauseManager.isPaused().Value && !IsPressingConfirm()));
                }

                anim.SetBool("Activated", false);
                isInDialogue = false;
            }
        }

        IEnumerator TypeSentence(string sentence)
        {
            text.text = "";
            bool skip = false;

            for (int i = 0; i < sentence.Length; i++)
            {
                if (PauseManager.isPaused().Value)
                    yield return new WaitWhile(() => PauseManager.isPaused().Value);

                text.text += sentence[i];

                every4thLetter = (every4thLetter + 1) % 4;
                if (!skip && every4thLetter == 0)
                    PlayRandomSound();

                if (IsPressingConfirm())
                    skip = true;

                if (!skip)
                    yield return new WaitForSeconds(1f / 60f);
            }
        }

        void PlayRandomSound()
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    currentAudioSource = i;
                    break;
                }
                if (i == audioSources.Count - 1)
                    currentAudioSource = (currentAudioSource + 1) % audioSources.Count;

            }
            AudioSource audioSource = audioSources[currentAudioSource];

            if (!audioSource.isPlaying)
            {
                audioSource.pitch = 1f + Random.Range(-.2f, .2f);
                audioSource.clip = currentAudioClips[Random.Range(0, currentAudioClips.Length)];
                audioSource.Play();
            }
        }

        AudioClip[] GetRightAudioClips(bool creepy)
        {
            if (creepy)
                return audioClipsCreepy;
            return audioClipsNormal;
        }

        void Reset()
        {
            text.text = "";
            text.fontStyle = FontStyle.Normal;
        }

        bool IsPressingConfirm()
        {
            if (InputSettings.PressingUse())
                return true;

            if (InputSettings.PressingConfirm() && !isPressing)
            {
                isPressing = true;
                return true;
            }
            if (!InputSettings.PressingConfirm() && isPressing)
                isPressing = false;
            return false;
        }
    }
}