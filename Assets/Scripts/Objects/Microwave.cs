using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Microwave : MonoBehaviour, Useable
    {
        public Outline outline;
        private OutlineHelper outlineHelper;
        public Animator anim;
        new public Light light;

        public GameObject h15Trigger;
        public H10FoodFlesh fleshScript;

        [Space(10)]
        new public AudioSource audio;
        public AudioClip sfxOpen;
        public AudioClip sfxClose;
        public AudioClip sfxStarting;
        public AudioClip sfxFinishing;
        public AudioClip sfxActive;

        bool open;
        bool turnedOn;
        bool locked = true;

        private void Awake()
        {
            outlineHelper = new OutlineHelper(this, outline);
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        public void Use()
        {
            if (locked)
                return;
            if (open)
                Close();
            else if (turnedOn)
                TurnOff();
            else
                Open();
        }

        public void H14Event()
        {
            Close();
            TurnOn();
            StartCoroutine(GameController.Instance.playerEventManager.FocusObject(false, transform, 2f));

            h15Trigger.SetActive(true);

            StartCoroutine(StartNextTaskDelayedFast());
        }

        private IEnumerator StartNextTaskDelayedFast()
        {
            yield return new WaitForSeconds(15f);
            GameController.Instance.storyManager.StartTask("T12");
            yield return new WaitForSeconds(15f);
            if (turnedOn)
                TurnOff();
        }

        public void H15Event()
        {
            StartCoroutine(H15EventEnumerator());
        }

        private IEnumerator H15EventEnumerator()
        {
            fleshScript.ShowFlesh(true);

            yield return new WaitForSeconds(3f);

            fleshScript.ShowFlesh(false);
            locked = false;
        }

        void Open()
        {
            if (open)
                return;

            anim.SetBool("open", true);
            open = true;

            audio.clip = sfxOpen;
            audio.loop = false;
            audio.Play();
        }

        public void Close()
        {
            if (!open)
                return;

            anim.SetBool("open", false);
            open = false;

            audio.clip = sfxClose;
            audio.loop = false;
            audio.PlayDelayed(0.32f);
        }

        public void LookingAt()
        {
            outlineHelper.ShowOutline();
        }

        public void TurnOn()
        {
            if (open || turnedOn)
                return;

            turnedOn = true;

            audio.clip = sfxStarting;
            audio.loop = false;
            audio.Play();
            StartCoroutine(TurnOnDelay());
        }

        void TurnOff()
        {
            if (!turnedOn)
                return;

            anim.SetBool("on", false);
            turnedOn = false;

            light.enabled = false;
            audio.clip = sfxFinishing;
            audio.loop = false;
            audio.Play();
        }

        IEnumerator TurnOnDelay()
        {
            yield return null;
            yield return new WaitWhile(() => audio.isPlaying);

            if (turnedOn)
            {
                anim.SetBool("on", true);
                light.enabled = true;

                audio.clip = sfxActive;
                audio.loop = true;
                audio.Play();
            }
        }
    }
}
