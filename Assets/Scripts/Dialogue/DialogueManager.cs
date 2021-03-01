using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class DialogueManager : MonoBehaviour//, Pausing
    {
        public Animator anim;
        public Text text;
        public Text nameText;
        public GameObject audioSourceObject;
        public int audioSourcesCount = 25;
        public UnityEngine.EventSystems.EventSystem eventSystem;
        public GameObject[] choices;
        public int pressedChoice = -1;
        public string ending = null;
        [Space(10)]
        public AudioClip[] audioClipsNormal;
        public AudioClip[] audioClipsCreepy;
        AudioClip[] currentAudioClips;

        int every4thLetter = -1;
        int currentAudioSource = -1;
        List<AudioSource> audioSources;

        bool isPressing = false;
        bool isInDialogue = false;

        void Start()
        {
            audioSources = new List<AudioSource>();
            audioSources.Add(audioSourceObject.GetComponent<AudioSource>());
            for (int i = 0; i < audioSourcesCount - 1; i++)
                audioSources.Add(Instantiate(audioSourceObject, audioSourceObject.transform.position,
                    audioSourceObject.transform.rotation, transform).GetComponent<AudioSource>());
        }

        public IEnumerator StartDialogue(DialogueNode dialogue)
        {
            if (!isInDialogue)
            {
                isInDialogue = true;
                Reset();
                anim.SetBool("Activated", true);
                yield return new WaitForSeconds(.5f);

                while (true)
                {
                    nameText.text = dialogue.currentNameText;
                    currentAudioClips = GetRightAudioClips(dialogue.currentVoice);

                    if (PauseManager.isPaused().Value)
                        yield return new WaitWhile(() => PauseManager.isPaused().Value);

                    yield return TypeSentence(dialogue.currentText);

                    if (Input.GetKeyDown(KeyCode.L))
                        break;

                    if (dialogue.currentChoices != null && dialogue.currentChoices.Count > 0)
                    {
                        choices[0].GetComponentInChildren<Text>().text = dialogue.currentChoices[0];
                        choices[0].SetActive(true);
                        eventSystem.SetSelectedGameObject(choices[0]);

                        switch (dialogue.nextNodes.Count)
                        {
                            case 1:
                                break;
                            case 2:
                                choices[1].GetComponentInChildren<Text>().text = dialogue.currentChoices[1];
                                choices[1].SetActive(true);
                                break;
                            case 3:
                                choices[2].GetComponentInChildren<Text>().text = dialogue.currentChoices[2];
                                choices[2].SetActive(true);
                                goto case 2;
                            default:
                                Debug.LogWarning("More than 3 choices are not implemented yet.");
                                break;
                        }
                        anim.SetBool("Choices", true);
                        pressedChoice = -1;
                        yield return new WaitWhile(() => pressedChoice == -1 || IsPressingConfirm());

                        if (pressedChoice < 1 || pressedChoice > 3)
                            Debug.LogError("unsupported choice");

                        dialogue = dialogue.nextNodes[pressedChoice - 1];
                        anim.SetBool("Choices", false);
                        eventSystem.SetSelectedGameObject(null);
                        choices[0].SetActive(false);
                        choices[1].SetActive(false);
                        choices[2].SetActive(false);
                    }
                    else
                    {
                        yield return new WaitUntil(() => (!PauseManager.isPaused().Value && IsPressingConfirm()));
                        yield return new WaitUntil(() => (!PauseManager.isPaused().Value && !IsPressingConfirm()));
                        if (dialogue.nextNodes.Count == 0)
                            break;
                        if (dialogue.nextNodes[0].currentText == null)
                        {
                            ending = dialogue.nextNodes[0].end;
                            break;
                        }


                        dialogue = dialogue.nextNodes[0];
                    }
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

                if (sentence[i] == '§' && i + 1 < sentence.Length && TextCode(sentence[i + 1]))
                {
                    i++;
                    continue;
                }

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

        AudioClip[] GetRightAudioClips(DialogueVoice voice)
        {
            switch (voice)
            {
                case DialogueVoice.Normal:
                    return audioClipsNormal;

                case DialogueVoice.Creepy:
                    return audioClipsCreepy;

                default:
                    return audioClipsNormal;
            }
        }

        bool TextCode(char code)
        {
            switch (code)
            {
                case 'i':
                    text.fontStyle = FontStyle.Italic;
                    return true;
                case 'n':
                    text.fontStyle = FontStyle.Normal;
                    return true;
                case 'b':
                    text.fontStyle = FontStyle.Bold;
                    return true;
                default:
                    return false;
            }
        }

        void Reset()
        {
            text.text = "";
            text.fontStyle = FontStyle.Normal;
            nameText.text = "";
            ending = null;
        }

        bool IsPressingConfirm()
        {
            if (Input.GetKeyDown(GlobalSettings.keyUse) || Input.GetKeyDown(GlobalSettings.keyUse2))
                return true;

            if (GlobalSettings.PressingConfirm() && !isPressing)
            {
                isPressing = true;
                return true;
            }
            if (!GlobalSettings.PressingConfirm() && isPressing)
                isPressing = false;
            return false;
        }
/*
        void Pausing.Pause()
        {

        }


        void Pausing.UnPause()
        {
            eventSystem.SetSelectedGameObject(choices[0]);
        }*/
    }
}