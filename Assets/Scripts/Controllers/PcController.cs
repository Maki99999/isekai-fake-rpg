using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Default
{
    public class PcController : MonoBehaviour, Useable
    {
        bool inTransition = false;
        bool inPcMode = false;

        [SerializeField, Range(0.0f, 1.0f)]
        private float immersedValue;
        public float maxImmersionTime = 10f;

        public Transform pcLookTransform;
        public float maxPcLookDistance = 1f;
        public Transform standUpTransform;
        public AudioMixer gameAudio;

        void Useable.Use()
        {
            if (inTransition)
                return;
            if (!inPcMode)
                StartCoroutine(ToPcMode());
        }

        private void Update()
        {
            if (inPcMode && !inTransition && (Input.GetAxis("Cancel") > 0 || Input.GetKey(GlobalSettings.keyEscapeDebug)))
            {
                StartCoroutine(ToNonPcMode());
            }
        }

        //TODO: Make Schieberegler: 0 is right in front of screen, 1 is half a meter or so in front of it
        //TODO: Transport AudioSources to pc and back to the main game

        IEnumerator ToPcMode()
        {
            inTransition = true;
            GameController.Instance.metaPlayer.SetCanMove(false);
            yield return GameController.Instance.metaPlayer.MovePlayer(pcLookTransform, 100, true);
            GameController.Instance.player.SetCanMove(true);
            inTransition = false;
            inPcMode = true;
            StartCoroutine(Immerse());
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            GameController.Instance.player.SetCanMove(false);
            StartCoroutine(AudioFiltersSmoothIn());

            yield return GameController.Instance.metaPlayer.MovePlayer(standUpTransform, 100);

            GameController.Instance.metaPlayer.SetCanMove(true);
            inTransition = false;
            inPcMode = false;
        }

        IEnumerator AudioFiltersSmoothIn()
        {
            for (int i = 0; i <= 100; i++)
            {
                float smoothF = Mathf.SmoothStep(0f, 1f, i / 100f);

                gameAudio.SetFloat("gameVolume", smoothF * -20f);
                gameAudio.SetFloat("eqOctaveRange", smoothF * 5);
                gameAudio.SetFloat("HighpassCutoff", smoothF * 150);
                gameAudio.SetFloat("LowpassCutoff", 22000f - (smoothF * (22000 - 450)));

                yield return new WaitForSeconds(1f / 60f);
            }
        }

        IEnumerator Immerse()
        {
            float currMaxImmersionTime = maxImmersionTime;
            for (float f = 0; f <= currMaxImmersionTime; f += 0.1f)
            {
                if (!inPcMode)
                    break;

                float smoothF = Mathf.SmoothStep(0f, 1f, f / currMaxImmersionTime);

                gameAudio.SetFloat("gameVolume", -20f + smoothF * 20f);
                gameAudio.SetFloat("eqOctaveRange", 5f - smoothF * 5f);
                gameAudio.SetFloat("HighpassCutoff", 150f - smoothF * 150f);
                gameAudio.SetFloat("LowpassCutoff", 450f + (smoothF * (22000 - 450)));

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
