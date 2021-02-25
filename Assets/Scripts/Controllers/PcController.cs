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

        public Transform pcLookTransform;
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

        //TODO: Make Schieberegler: 0 is right in front of screen, 1 ist half a meter or so in front of it
        //TODO: Transport AudioSources to pc and back to the main game

        IEnumerator ToPcMode()
        {
            inTransition = true;
            GameController.Instance.metaPlayer.SetCanMove(false);
            yield return GameController.Instance.metaPlayer.MovePlayer(pcLookTransform, 100, true);

            //TODO: Audio Effects Smoothing (3 coroutines)
            gameAudio.SetFloat("eqOctaveRange", 0);
            gameAudio.SetFloat("HighpassCutoff", 0);
            gameAudio.SetFloat("LowpassCutoff", 22000);
            GameController.Instance.player.SetCanMove(true);
            inTransition = false;
            inPcMode = true;
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            GameController.Instance.player.SetCanMove(false);
            yield return GameController.Instance.metaPlayer.MovePlayer(standUpTransform, 100);
            gameAudio.SetFloat("eqOctaveRange", 5);
            gameAudio.SetFloat("HighpassCutoff", 150);
            gameAudio.SetFloat("LowpassCutoff", 450);
            GameController.Instance.metaPlayer.SetCanMove(true);
            inTransition = false;
            inPcMode = false;
        }
    }
}
