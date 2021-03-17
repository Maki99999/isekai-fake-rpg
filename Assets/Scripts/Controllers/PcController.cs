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

        [Space(10)]
        public Transform lookAt;
        public Animator headAnim;

        bool isLooking = false;
        bool inTransition = false;
        bool transitionsToPcMode;   //to use in combination with inTransition

        bool immersedValueIsRegular = true;
        float immersedValueRegular;

        void Useable.Use()
        {
            if (inTransition)
                return;
            if (!GameController.Instance.inPcMode)
                StartCoroutine(ToPcMode());
        }

        void Useable.LookingAt() { }

        private void Start()
        {
            ToPcModeInstant();
            immersedValueRegular = GameController.Instance.inPcMode ? 1 : 0;
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
            else
            {
                if (!inTransition)
                {
                    if (InputSettings.PressingStand())
                        StartCoroutine(ToNonPcMode());
                    else
                        LookingAt();
                }
            }
        }

        void LookingAt()
        {
            if (InputSettings.PressingLook())
            {
                if (!isLooking)
                {
                    isLooking = true;
                    StartCoroutine(LookAtTransition(false));
                }
            }
            else
            {
                if (isLooking)
                {
                    isLooking = false;
                    StartCoroutine(LookAtTransition(true));
                }
            }
        }

        IEnumerator LookAtTransition(bool reversed)
        {
            if (!reversed)
                immersedValueIsRegular = false;

            float oldImmVal = ImmersedValue;
            float newImmValNormal = 0.5f;

            Vector3 oldRot = GameController.Instance.metaPlayer.GetRotation();
            Vector3 newRot;
            if (reversed)
                newRot = Quaternion.LookRotation(transform.position - GameController.Instance.metaPlayer.eyeHeightTransform.position).eulerAngles;
            else
                newRot = Quaternion.LookRotation(lookAt.position - GameController.Instance.metaPlayer.eyeHeightTransform.position).eulerAngles;

            float rate = 1f / 0.35f;
            for (float f = 0; f <= 1f && !inTransition && ((reversed && !isLooking) || (!reversed && isLooking)); f += Time.deltaTime * rate)
            {
                GameController.Instance.metaPlayer.SetRotationLerp(oldRot, newRot, Mathf.SmoothStep(0f, 1f, f));
                ImmersedValue = Mathf.Lerp(oldImmVal, reversed ? immersedValueRegular : newImmValNormal, f);
                yield return null;
            }
            if (reversed)
                immersedValueIsRegular = true;
        }

        public float ImmersedValue
        {
            get { return _immersedValue; }
            set
            {
                _immersedValue = value;

                gameAudio.SetFloat("eqOctaveRange", Mathf.Lerp(5f, 0f, _immersedValue));
                gameAudio.SetFloat("HighpassCutoff", Mathf.Lerp(150f, 0f, _immersedValue));
                gameAudio.SetFloat("LowpassCutoff", Mathf.Lerp(450f, 22000f, _immersedValue));

                gameAudio.SetFloat("metaVolume", Mathf.Lerp(0f, -80f, _immersedValue));
                gameAudio.SetFloat("gameVolume", Mathf.Lerp(-20f, 0f, _immersedValue));

                Vector3 pcLookTransformNoOffset = pcLookTransform.position - Vector3.up * (GameController.Instance.metaPlayer.heightNormal / 2 - GameController.Instance.metaPlayer.camOffsetHeight);
                GameController.Instance.metaPlayer.transform.position = Vector3.Lerp(pcLookTransformNoOffset + Vector3.forward * maxPcLookDistance, pcLookTransformNoOffset, _immersedValue);

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
            headAnim.SetBool("Wobble", true);

            StartCoroutine(Immerse(false, maxImmersionTime));
        }

        public void ToPcModeInstant()
        {
            GameController.Instance.metaPlayer.SetCanMove(false);
            GameController.Instance.metaPlayer.TeleportPlayer(pcLookTransform, true, maxPcLookDistance * Vector3.forward);
            GameController.Instance.gamePlayer.SetCanMove(true);

            GameController.Instance.inPcMode = true;
            headAnim.SetBool("Wobble", true);
            ImmersedValue = 1f;
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            transitionsToPcMode = false;
            GameController.Instance.gamePlayer.SetCanMove(false);

            StartCoroutine(Immerse(true, 2f));

            yield return GameController.Instance.metaPlayer.MoveRotatePlayer(standUpTransform, 2f);

            GameController.Instance.metaPlayer.SetCanMove(true);
            inTransition = false;
            GameController.Instance.inPcMode = false;
            headAnim.SetBool("Wobble", false);
        }

        IEnumerator Immerse(bool reverse, float seconds)
        {
            float startValue = ImmersedValue;
            float rate = 1f / seconds;
            for (float f = 0; f <= 1f && (!inTransition || !reverse && transitionsToPcMode || reverse && !transitionsToPcMode); f += Time.deltaTime * rate) // (!reverse && !(inTransition && !transitionsToPcMode) || reverse && !(inTransition && transitionsToPcMode))
            {
                float valueNew;
                if (reverse)
                    valueNew = Mathf.SmoothStep(startValue, 0f, f);
                else
                    valueNew = Mathf.SmoothStep(startValue, 1f, f);

                if (immersedValueIsRegular)
                    ImmersedValue = valueNew;
                else
                    immersedValueRegular = valueNew;

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
