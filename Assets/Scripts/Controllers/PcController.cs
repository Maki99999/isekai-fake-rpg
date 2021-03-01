using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Default
{
    public class PcController : MonoBehaviour, Useable
    {
        [SerializeField, Range(0.0f, 1.0f)]
        private float _immersedValue;
        public float maxImmersionTime = 10f;

        public Transform pcLookTransform;
        public float maxPcLookDistance = 1f;
        public Transform standUpTransform;
        public AudioMixer gameAudio;

        bool inTransition = false;
        bool transitionsToPcMode;   //to use in combination with inTransition

        void Useable.Use()
        {
            if (inTransition)
                return;
            if (!GameController.Instance.inPcMode)
                StartCoroutine(ToPcMode());
        }

        private void Start()
        {
            ImmersedValue = 0f;
        }

        private void Update()
        {
            if (GameController.Instance.inPcMode && !inTransition && (Input.GetAxis("Cancel") > 0 || Input.GetKey(GlobalSettings.keyEscapeDebug)))
            {
                StartCoroutine(ToNonPcMode());
            }

            //TODO: with Schieberegler: 0 is right in front of screen, 1 is half a meter or so in front of it
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

        public float ImmersedValue
        {
            get { return _immersedValue; }
            set
            {
                _immersedValue = value;

                gameAudio.SetFloat("eqOctaveRange", 5 - _immersedValue * 5);
                gameAudio.SetFloat("HighpassCutoff", 150 - _immersedValue * 150);
                gameAudio.SetFloat("LowpassCutoff", 450 + (_immersedValue * (22000 - 450)));

                GameController.Instance.gameAudioFxStrength = 1f - _immersedValue;
            }
        }

        IEnumerator ToPcMode()
        {
            inTransition = true;
            transitionsToPcMode = true;
            GameController.Instance.metaPlayer.SetCanMove(false);

            yield return GameController.Instance.metaPlayer.MovePlayer(pcLookTransform, 100, true, maxPcLookDistance * Vector3.forward);

            GameController.Instance.player.SetCanMove(true);
            inTransition = false;
            GameController.Instance.inPcMode = true;

            StartCoroutine(Immerse());
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            transitionsToPcMode = false;
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
                if (inTransition && transitionsToPcMode)
                    break;

                ImmersedValue = Mathf.SmoothStep(1f, 0f, i / 100f);

                float currVolume;
                gameAudio.GetFloat("gameVolume", out currVolume);
                gameAudio.SetFloat("gameVolume", Mathf.Lerp(-20f, currVolume, ImmersedValue));

                yield return new WaitForSeconds(1f / 60f);
            }
        }

        IEnumerator Immerse()
        {
            Vector3 pcLookTransformNoOffset = pcLookTransform.position - Vector3.up * (GameController.Instance.metaPlayer.heightNormal / 2 - GameController.Instance.metaPlayer.camOffsetHeight);
            Vector3 pcLookTransformOffset = pcLookTransformNoOffset + Vector3.forward * maxPcLookDistance;
            float currVolume;
            gameAudio.GetFloat("gameVolume", out currVolume);

            float rate = 1f / maxImmersionTime;
            for (float f = 0; f <= 1f; f += Time.deltaTime * rate)
            {
                if (inTransition && !transitionsToPcMode)
                    break;

                ImmersedValue = Mathf.SmoothStep(0f, 1f, f);

                gameAudio.SetFloat("gameVolume", Mathf.Lerp(currVolume, 0f, ImmersedValue));

                GameController.Instance.metaPlayer.transform.position = Vector3.Lerp(pcLookTransformOffset, pcLookTransformNoOffset, ImmersedValue);

                yield return null;
            }
        }

        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir.normalized);
            return Vector3.Dot(perp, up);
        }
    }
}
