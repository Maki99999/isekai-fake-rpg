using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Default
{
    public class PcController : MonoBehaviour, Useable
    {
        bool inTransition = false;

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
            if (!GameController.Instance.inPcMode)
                StartCoroutine(ToPcMode());
        }

        private void Update()
        {
            if (GameController.Instance.inPcMode && !inTransition && (Input.GetAxis("Cancel") > 0 || Input.GetKey(GlobalSettings.keyEscapeDebug)))
            {
                StartCoroutine(ToNonPcMode());
            }
            if (!GameController.Instance.inPcMode)
            {
                float minDist = 2f;
                float maxDist = 7f;
                float maxDistY = 1f;

                float newVolume;
                if (Mathf.Abs(transform.position.y - GameController.Instance.metaPlayer.transform.position.y) > maxDistY)
                    newVolume = -80f;
                else
                {
                    float dist = Mathf.Abs((transform.position - GameController.Instance.metaPlayer.transform.position).magnitude);
                    float percent = Mathf.Clamp((dist - minDist) / (maxDist - minDist), 0f, 1f);
                    newVolume = -20f - percent * 60f;
                }

                gameAudio.SetFloat("gameVolume", newVolume);

                GameController.Instance.gameAudioPan = AngleDir(GameController.Instance.metaPlayer.transform.forward,
                        transform.position - GameController.Instance.metaPlayer.transform.position,
                        Vector3.up);
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
            GameController.Instance.inPcMode = true;
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
            GameController.Instance.inPcMode = false;
        }

        IEnumerator AudioFiltersSmoothIn()
        {
            for (int i = 0; i <= 100; i++)
            {
                float smoothF = Mathf.SmoothStep(0f, 1f, i / 100f);

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
                if (!GameController.Instance.inPcMode)
                    break;

                float smoothF = Mathf.SmoothStep(0f, 1f, f / currMaxImmersionTime);

                float currVolume;
                gameAudio.GetFloat("gameVolume", out currVolume);

                gameAudio.SetFloat("gameVolume", Mathf.Lerp(currVolume, 0f, smoothF));
                gameAudio.SetFloat("eqOctaveRange", 5f - smoothF * 5f);
                gameAudio.SetFloat("HighpassCutoff", 150f - smoothF * 150f);
                gameAudio.SetFloat("LowpassCutoff", 450f + (smoothF * (22000 - 450)));

                yield return new WaitForSeconds(0.1f);
            }
        }

        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir.normalized);
            return Vector3.Dot(perp, up);
        }
    }
}
