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

        public GameObject FakePause;

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
            FakePause.SetActive(false);
        }

        private void Update()
        {
            if (PauseManager.isPaused().Value || !GameController.Instance.inPcMode)
            {
                if (!FakePause.activeSelf)
                    FakePause.SetActive(true);
            }
            else if (FakePause.activeSelf)
                FakePause.SetActive(false);

            if (GameController.Instance.inPcMode && !inTransition && InputSettings.PressingStand())
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

            gameAudio.SetFloat("metaVolume", Mathf.Lerp(0f, -80f, ImmersedValue));
            gameAudio.SetFloat("gameVolume", Mathf.Lerp(-20f, 0f, ImmersedValue));
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

            yield return GameController.Instance.metaPlayer.MoveRotatePlayer(pcLookTransform, 2f, true, maxPcLookDistance * Vector3.forward);

            GameController.Instance.gamePlayer.SetCanMove(true);
            inTransition = false;
            GameController.Instance.inPcMode = true;

            StartCoroutine(Immerse());
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            transitionsToPcMode = false;
            GameController.Instance.gamePlayer.SetCanMove(false);

            StartCoroutine(AudioFiltersSmoothIn());

            yield return GameController.Instance.metaPlayer.MoveRotatePlayer(standUpTransform, 2f);

            GameController.Instance.metaPlayer.SetCanMove(true);
            inTransition = false;
            GameController.Instance.inPcMode = false;
        }

        IEnumerator AudioFiltersSmoothIn()
        {
            float rate = 1f / 2f;
            for (float f = 0; f <= 1f && !(inTransition && transitionsToPcMode); f += Time.deltaTime * rate)
            {
                ImmersedValue = Mathf.SmoothStep(1f, 0f, f);

                yield return null;
            }
        }

        IEnumerator Immerse()
        {
            Vector3 pcLookTransformNoOffset = pcLookTransform.position - Vector3.up * (GameController.Instance.metaPlayer.heightNormal / 2 - GameController.Instance.metaPlayer.camOffsetHeight);
            Vector3 pcLookTransformOffset = pcLookTransformNoOffset + Vector3.forward * maxPcLookDistance;

            float rate = 1f / maxImmersionTime;
            for (float f = 0; f <= 1f && !(inTransition && !transitionsToPcMode); f += Time.deltaTime * rate)
            {
                ImmersedValue = Mathf.SmoothStep(0f, 1f, f);

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
