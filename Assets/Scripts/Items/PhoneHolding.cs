using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PhoneHolding : ItemHoldable
    {
        public GameObject flashlight;
        public AudioSource tapSound;
        public Animator phoneAnim;

        [Header("H1Call")]
        public GameObject phoneCallCanvasObj1;
        public GameObject phoneCallCanvasObj2;
        public PcController pcController;
        public AudioSource callAudio;
        public AudioClip sfxRinging;
        public AudioClip[] sfxCalls;
        int nextCallClip = 0;
        public AudioClip sfxDisconnected;
        public AudioClip sfxAcceptCall;

        bool pressedLastFrame = false;
        bool isActive = false;

        public void H1Call()
        {
            StartCoroutine(H1CallAnim());
        }

        public IEnumerator H1CallAnim()
        {
            callAudio.clip = sfxRinging;
            callAudio.loop = true;
            callAudio.Play();
            phoneCallCanvasObj1.SetActive(true);
            phoneCallCanvasObj2.SetActive(true);

            if (GameController.Instance.inPcMode)
            {
                GameController.Instance.playerEventManager.FreezePlayer(true, true);
                pcController.ImmerseBreak(true);
                StartCoroutine(pcController.HidePhone());
            }
            else
                GameController.Instance.playerEventManager.FreezePlayer(false, true);

            yield return TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.up * 0.3f, 1f);
            yield return new WaitForSeconds(2f);
            StartCoroutine(TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.left * 0.3f + Vector3.up * 0.3f, 1f));

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
            phoneCallCanvasObj1.SetActive(false);
            phoneCallCanvasObj2.SetActive(false);

            StartCoroutine(TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.up * 0.3f, 1f));
            yield return new WaitForSeconds(0.7f);
            callAudio.Stop();
            yield return new WaitForSeconds(0.6f);
            yield return TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.zero, 1f);

            if (GameController.Instance.inPcMode)
            {
                GameController.Instance.playerEventManager.FreezePlayer(true, false);
                pcController.ImmerseBreak(false);
                pcController.ShowPhone();
            }
            else
                GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        public override MoveData UseItem(MoveData inputData)
        {
            if (!pressedLastFrame && inputData.axisPrimary > 0)
            {
                tapSound.Play();
                isActive = !isActive;
                flashlight.SetActive(isActive);
            }
            pressedLastFrame = inputData.axisPrimary > 0;
            return inputData;
        }

        public override void OnEquip()
        {
            isActive = true;
            flashlight.SetActive(true);
            phoneAnim.SetBool("Unlock", true);

            if (isActiveAndEnabled)
                StartCoroutine(TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.up * 0.2f, 0.5f));
            else
                GameController.Instance.metaPlayer.RemoveItem(this);
        }

        public override void OnUnequip()
        {
            isActive = false;
            flashlight.SetActive(false);
            phoneAnim.SetBool("Unlock", false);

            if (isActiveAndEnabled)
                StartCoroutine(TransformOperations.MoveToLocal(transform.GetChild(0), Vector3.zero, 0.5f));
        }
    }
}
