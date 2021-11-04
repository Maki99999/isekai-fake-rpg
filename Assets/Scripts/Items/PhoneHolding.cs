using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PhoneHolding : ItemHoldable, ISaveDataObject
    {
        public GameObject flashlight;
        public GameObject flashlightScreen;
        public PhoneTimer timer;
        public GameObject clockScreen;
        public AudioSource tapSound;
        public Transform phoneChild;
        public Animator phoneAnim;

        [Header("H1Call")]
        public GameObject phoneCallCanvasObj;
        public PcController pcController;
        public AudioSource callAudio;
        public AudioClip sfxRinging;
        public AudioClip[] sfxCalls;
        int nextCallClip = 0;
        public AudioClip sfxDisconnected;
        public AudioClip sfxAcceptCall;

        private State currentState = State.UNEQUIPPED;
        private bool pressedLastFrame = false;
        private Coroutine showHideRoutine = null;
        private bool currentlyLookingAtInPcMode = false;

        public string saveDataId => "H1Call";

        public void H1Call()
        {
            StartCoroutine(H1CallAnim());
        }

        public IEnumerator H1CallAnim()
        {
            State prevState = currentState;
            currentState = State.CALL;

            callAudio.clip = sfxRinging;
            callAudio.loop = true;
            callAudio.Play();
            phoneCallCanvasObj.SetActive(true);

            if (GameController.Instance.inPcMode)
            {
                GameController.Instance.playerEventManager.FreezePlayer(true, true);
                pcController.ImmerseBreak(true);
                Hide();
            }
            else
                GameController.Instance.playerEventManager.FreezePlayer(false, true);

            yield return TransformOperations.MoveToLocal(phoneChild, Vector3.up * 0.3f, 1f);
            yield return new WaitForSeconds(2f);
            StartCoroutine(TransformOperations.MoveToLocal(phoneChild, Vector3.left * 0.3f + Vector3.up * 0.3f, 1f));

            yield return new WaitForSeconds(0.1f);
            callAudio.clip = sfxAcceptCall;
            callAudio.loop = false;
            callAudio.Play();
            yield return new WaitForSeconds(0.4f);
            callAudio.clip = sfxCalls[nextCallClip];
            callAudio.loop = true;
            callAudio.Play();
            nextCallClip = (nextCallClip + 1) % sfxCalls.Length;

            yield return new WaitUntil(() => InputSettings.PressingUse());
            yield return new WaitWhile(() => InputSettings.PressingUse());

            callAudio.clip = sfxDisconnected;
            callAudio.Play();
            phoneCallCanvasObj.SetActive(false);

            StartCoroutine(TransformOperations.MoveToLocal(phoneChild, Vector3.up * 0.3f, 1f));
            yield return new WaitForSeconds(0.7f);
            callAudio.Stop();
            yield return new WaitForSeconds(0.6f);
            yield return TransformOperations.MoveToLocal(phoneChild, Vector3.zero, 1f);

            if (GameController.Instance.inPcMode)
            {
                GameController.Instance.playerEventManager.FreezePlayer(true, false);
                pcController.ImmerseBreak(false);
                CustomPos(pcController.phonePos);
            }
            else
                GameController.Instance.playerEventManager.FreezePlayer(false, false);

            currentState = prevState;
        }

        public void ActivateFlashlight()
        {
            currentState = State.FLASHLIGHT_ON;
            flashlightScreen.SetActive(true);
            flashlight.SetActive(true);
        }

        public void ActivateClockApp()
        {
            clockScreen.SetActive(true);
        }

        public void StartTimer(int minutes, float timeMultiplier)
        {
            timer.StartTimer(minutes, timeMultiplier);
        }

        public void SkipTime(float seconds)
        {
            timer.SkipTime(seconds);
        }

        public void ShowScreen(bool show)
        {
            phoneAnim.SetBool("Unlock", show);
            currentlyLookingAtInPcMode = show;
        }

        public bool IsLookingAtPhone()
        {
            return currentState == State.EQUIPPED_VISIBLE || currentlyLookingAtInPcMode;
        }

        public override MoveData UseItem(MoveData inputData)
        {
            bool pressedThisFrame = !pressedLastFrame && inputData.axisPrimary > 0;

            if (currentState == State.FLASHLIGHT_ON || currentState == State.FLASHLIGHT_OFF)
            {
                if (pressedThisFrame)
                {
                    tapSound.Play();
                    currentState = currentState == State.FLASHLIGHT_ON ? State.FLASHLIGHT_OFF : State.FLASHLIGHT_ON;
                    flashlight.SetActive(currentState == State.FLASHLIGHT_ON);
                }
            }
            else if (currentState == State.EQUIPPED_VISIBLE || currentState == State.EQUIPPED_HIDDEN)
            {
                if (pressedThisFrame)
                {
                    Hide(currentState == State.EQUIPPED_HIDDEN);
                }
            }

            pressedLastFrame = inputData.axisPrimary > 0;
            return inputData;
        }

        public override void OnEquip()
        {
            if (currentState == State.UNEQUIPPED)
                currentState = State.EQUIPPED_VISIBLE;
            pcController.lookAtPhone = true;
            phoneAnim.SetBool("Unlock", true);

            Hide(true);
        }

        public override void OnUnequip()
        {
            flashlightScreen.SetActive(false);
            flashlight.SetActive(false);
            phoneAnim.SetBool("Unlock", false);
            if (!GameController.Instance.inPcMode && (currentState != State.FLASHLIGHT_ON || currentState != State.FLASHLIGHT_OFF))
            {
                currentState = State.UNEQUIPPED;
                pcController.lookAtPhone = false;
            }

            Hide(false);
        }

        public void Hide(bool reversed = false)
        {
            if (currentState == State.EQUIPPED_VISIBLE || currentState == State.EQUIPPED_HIDDEN)
                currentState = reversed ? State.EQUIPPED_VISIBLE : State.EQUIPPED_HIDDEN;

            if (showHideRoutine != null)
                StopCoroutine(showHideRoutine);
            showHideRoutine = StartCoroutine(HideAnim(reversed));
        }

        IEnumerator HideAnim(bool reversed)
        {
            phoneChild.parent = transform;
            if (reversed)
                yield return TransformOperations.MoveToLocal(phoneChild, Vector3.up * 0.2f, new Vector3(180, 180, 90), 0.5f);
            else
                yield return TransformOperations.MoveToLocal(phoneChild, Vector3.zero, new Vector3(180, 180, 90), 0.5f);
        }

        public void CustomPos(Transform pos, bool instant = false)
        {
            if (instant)
            {
                phoneChild.parent = pos;
                phoneChild.position = pos.position;
                phoneChild.rotation = pos.rotation;
                return;
            }

            if (showHideRoutine != null)
                StopCoroutine(showHideRoutine);
            showHideRoutine = StartCoroutine(CustomPosAnim(pos));
        }

        IEnumerator CustomPosAnim(Transform pos)
        {
            phoneChild.parent = pos;
            yield return TransformOperations.MoveTo(phoneChild, pos.position, pos.rotation, 1f);
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("nextCallClip", nextCallClip);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;
            nextCallClip = dictEntry.GetInt("nextCallClip", 0);
        }

        private enum State
        {
            UNEQUIPPED,
            CALL,
            EQUIPPED_VISIBLE,
            EQUIPPED_HIDDEN,
            FLASHLIGHT_ON,
            FLASHLIGHT_OFF
        }
    }
}
