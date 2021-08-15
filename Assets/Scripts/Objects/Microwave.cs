using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Microwave : MonoBehaviour, Useable
    {
        public Outline outline;
        public Animator anim;
        new public Light light;

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
        bool locked;

        void Update()
        {
            outline.enabled = false;
        }

        public void Use()
        {
            if (locked)
                return;
            if (open)
                Close();
            else
            {
                if (turnedOn)
                    TurnOff();
                else if (Random.value < 0.5f)
                    TurnOn();
                else
                    Open();
            }
        }

        public void H15Event()
        {
            StartCoroutine(H15EventEnumerator());
        }

        IEnumerator H15EventEnumerator()
        {
            fleshScript.ShowFlesh(true);
            locked = true;

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
            audio.Play();
        }

        public void Close()
        {
            if (!open)
                return;

            anim.SetBool("open", false);
            open = false;

            audio.clip = sfxClose;
            audio.PlayDelayed(0.32f);
        }

        public void LookingAt()
        {
            outline.enabled = true;
        }

        public void TurnOn()
        {
            if (open || turnedOn)
                return;

            turnedOn = true;

            audio.clip = sfxStarting;
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
                audio.Play();
            }
        }
    }
}
