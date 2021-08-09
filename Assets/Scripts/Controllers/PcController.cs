using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Default
{
    public class PcController : MonoBehaviour, Useable, UsesPower
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

        [Space(20)]
        public GameObject screen;
        public Renderer pcMesh;
        public Material matPCaccent;
        public Material matPC;
        public AudioSource pcAudio1;
        public AudioSource pcAudio2;

        [Space(20)]
        public bool lookAtPhone = false;
        public Transform phone;
        public Transform phonePos;
        public Animator phoneAnim;

        bool powerOn = true;

        bool isLooking = false;
        bool inTransition = false;
        bool transitionsToPcMode;   //to use in combination with inTransition

        bool immersedValueIsRegular = true;
        float immersedValueRegular;

        void Useable.Use()
        {
            if (inTransition)
                return;
            if (!GameController.Instance.inPcMode && powerOn)
                StartCoroutine(ToPcMode());
        }

        void Useable.LookingAt() { }

        private void Start()
        {
            #region Editor
            //ToPcModeInstant();
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            GameController.Instance.playerEventManager.FreezePlayer(true, true);
            #endregion

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
                float maxDistY = 1.5f;

                float newVolume;
                if (Mathf.Abs(transform.position.y - GameController.Instance.metaPlayer.transform.position.y) > maxDistY || !powerOn)
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
                    if (InputSettings.PressingStand() && GameController.Instance.gamePlayer.CanMove())
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
            if (lookAtPhone)
                phoneAnim.SetBool("Unlock", !reversed);

            if (!reversed)
                immersedValueIsRegular = false;

            float oldImmVal = ImmersedValue;
            float newImmValNormal = Mathf.Min(ImmersedValue, 0.5f);

            ImmersedValue = newImmValNormal;
            Vector3 currentLookAtPos = (lookAtPhone ? phonePos : lookAt).position;
            Vector3 oldRot = GameController.Instance.metaPlayer.GetRotation();
            Vector3 newRot;
            if (reversed)
                newRot = Quaternion.LookRotation(transform.position - GameController.Instance.metaPlayer.eyeHeightTransform.position).eulerAngles;
            else
                newRot = Quaternion.LookRotation(currentLookAtPos - GameController.Instance.metaPlayer.eyeHeightTransform.position).eulerAngles;
            ImmersedValue = oldImmVal;

            float rate = 1f / 0.35f;
            for (float f = 0; f <= 1f && !inTransition && ((reversed && !isLooking) || (!reversed && isLooking)); f += Time.deltaTime * rate)
            {
                GameController.Instance.metaPlayer.SetRotationLerp(oldRot, newRot, Mathf.SmoothStep(0f, 1f, f));
                ImmersedValue = Mathf.Lerp(oldImmVal, reversed ? immersedValueRegular : newImmValNormal, f);
                yield return null;
            }
            if (!inTransition && ((reversed && !isLooking) || (!reversed && isLooking)))
            {
                GameController.Instance.playerEventManager.RotatePlayer(false, Quaternion.Euler(newRot), 0f);
                ImmersedValue = reversed ? immersedValueRegular : newImmValNormal;
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

                gameAudio.SetFloat("metaVolume", _immersedValue == 1f ? -80f : 20f * Mathf.Log10(1f - _immersedValue));
                if (powerOn)
                    gameAudio.SetFloat("gameVolume", Mathf.Lerp(-20f, 0f, _immersedValue));
                else
                    gameAudio.SetFloat("gameVolume", -80f);

                Vector3 pcLookTransformNoOffset = pcLookTransform.position - Vector3.up * (GameController.Instance.metaPlayer.heightNormal - GameController.Instance.metaPlayer.camOffsetHeight);
                GameController.Instance.metaPlayer.transform.position = Vector3.Lerp(pcLookTransformNoOffset + Vector3.forward * maxPcLookDistance, pcLookTransformNoOffset, _immersedValue);

                GameController.Instance.gameAudioFxStrength = 1f - _immersedValue;
            }
        }

        IEnumerator ToPcMode()
        {
            inTransition = true;
            transitionsToPcMode = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, true);

            phone.gameObject.SetActive(true);
            phone.position = GameController.Instance.metaPlayer.itemTransform.position;
            phone.rotation = phonePos.rotation;
            StartCoroutine(TransformOperations.MoveTo(phone, phonePos.position, phonePos.rotation, 1f));
            yield return GameController.Instance.metaPlayer.MoveRotatePlayer(pcLookTransform, 2f, true, maxPcLookDistance * Vector3.forward);

            GameController.Instance.playerEventManager.FreezePlayer(true, false);
            inTransition = false;
            GameController.Instance.inPcMode = true;
            headAnim.SetBool("Wobble", true);

            StartCoroutine(Immerse(false, maxImmersionTime));
        }

        public void ToPcModeInstant()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            GameController.Instance.metaPlayer.TeleportPlayer(pcLookTransform, true, maxPcLookDistance * Vector3.forward);
            GameController.Instance.playerEventManager.FreezePlayer(true, false);

            phone.gameObject.SetActive(true);
            phone.position = phonePos.position;
            phone.rotation = phonePos.rotation;

            GameController.Instance.inPcMode = true;
            headAnim.SetBool("Wobble", true);
            ImmersedValue = 1f;
            immersedValueRegular = 1f;
        }

        IEnumerator ToNonPcMode()
        {
            inTransition = true;
            transitionsToPcMode = false;
            GameController.Instance.gamePlayer.SetCanMove(false);

            StartCoroutine(Immerse(true, 2f));

            StartCoroutine(TransformOperations.MoveTo(phone, GameController.Instance.metaPlayer.itemTransform.position + Vector3.up * 0.1f, phonePos.rotation, 1f));
            yield return GameController.Instance.metaPlayer.MoveRotatePlayer(standUpTransform, 2f);
            phone.gameObject.SetActive(false);

            GameController.Instance.metaPlayer.SetCanMove(true);
            inTransition = false;
            GameController.Instance.inPcMode = false;
            headAnim.SetBool("Wobble", false);
        }

        IEnumerator Immerse(bool reverse, float seconds)
        {
            float startValue = ImmersedValue;
            float rate = 1f / seconds;
            for (float f = 0; f <= 1f && powerOn && (!inTransition || (!reverse && transitionsToPcMode) || (reverse && !transitionsToPcMode)); f += Time.deltaTime * rate) // (!reverse && !(inTransition && !transitionsToPcMode) || reverse && !(inTransition && transitionsToPcMode))
            {
                float valueNew;
                if (reverse)
                    valueNew = Mathf.SmoothStep(startValue, 0f, f);
                else
                    valueNew = Mathf.SmoothStep(startValue, 1f, f);

                if (immersedValueIsRegular)
                    ImmersedValue = valueNew;
                immersedValueRegular = valueNew;

                yield return null;
            }
        }

        public void SetPower(bool powerOn)
        {
            //TODO: GameAudio
            this.powerOn = powerOn;
            screen.SetActive(powerOn);
            pcAudio1.enabled = powerOn;
            pcAudio2.enabled = powerOn;

            PlayerController player = GameController.Instance.gamePlayer;
            Material[] materials = pcMesh.materials;

            if (powerOn)
            {
                player.transform.position += 1000f * Vector3.up;
                materials[1] = matPCaccent;
            }
            else
            {
                GameController.Instance.playerEventManager.FreezePlayer(true, true);
                ImmersedValue = 0f;
                immersedValueRegular = 0f;
                gameAudio.SetFloat("gameVolume", -80f);
                inTransition = false;

                player.transform.position += 1000f * Vector3.down;
                materials[1] = matPC;
            }

            pcMesh.materials = materials;
        }

        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir.normalized);
            return Vector3.Dot(perp, up);
        }
    }
}
