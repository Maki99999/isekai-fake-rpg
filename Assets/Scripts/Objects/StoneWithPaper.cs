using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StoneWithPaper : OutlineCreator
    {
        public Transform paperTransform;
        public Animator paperAnim;
        public AudioSource audio1;
        public AudioSource audio2;
        public CanvasGroup canvasGroup;

        public PhoneHolding phone;
        public Transform window;

        bool looking = false;
        bool firstLook = true;

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        void Use()
        {
            if (looking)
                return;
            StartCoroutine(LookAnim());
        }

        IEnumerator LookAnim()
        {
            looking = true;

            PlayerController player = GameController.Instance.metaPlayer;
            GameController.Instance.playerEventManager.FreezePlayer(false, true, true);
            StartCoroutine(GameController.Instance.playerEventManager.LookAt(false, transform.position, 1f));

            yield return new WaitUntil(() => !InputSettings.PressingUse());

            Vector3 origPos = paperTransform.position;
            Vector3 pos1 = origPos + paperTransform.up * 0.1f;
            Quaternion origRot = paperTransform.rotation;

            audio1.Play();
            yield return TransformOperations.MoveTo(paperTransform, pos1, origRot, 0.5f);
            paperAnim.SetBool("Fold", false);
            audio2.Play();
            StartCoroutine(PlayAudio2Delayed(1f));
            yield return TransformOperations.MoveTo(paperTransform, player.eyeHeightTransform.position + player.eyeHeightTransform.forward * 0.24f,
                        Quaternion.LookRotation(-player.eyeHeightTransform.forward, player.eyeHeightTransform.up), 1.5f);

            yield return new WaitForSeconds(0.5f);
            for (float f = 0f; f <= 1f; f += 0.5f * Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, f);
                yield return null;
            }

            yield return new WaitForSeconds(2f);
            if (firstLook)
            {
                yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Creepy...", "I think I'll call the police." });
                yield return new WaitWhile(() => InputSettings.PressingConfirm());
            }
            yield return new WaitUntil(() => InputSettings.PressingConfirm());
            yield return new WaitWhile(() => InputSettings.PressingConfirm());

            for (float f = 0f; f <= 1f; f += 2f * Time.deltaTime)
            {
                canvasGroup.alpha = Mathf.SmoothStep(1f, 0f, f);
                yield return null;
            }

            paperAnim.SetBool("Fold", true);
            audio2.Play();
            StartCoroutine(PlayAudio2Delayed(0.5f));
            yield return TransformOperations.MoveTo(paperTransform, pos1, origRot, 1.5f);
            audio1.Play();
            StartCoroutine(TransformOperations.MoveTo(paperTransform, origPos, origRot, 0.5f));

            if (firstLook)
            {
                StartCoroutine(GameController.Instance.playerEventManager.LookAt(false, window.position, 5f));
                yield return phone.H3Call();
            }

            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            firstLook = false;
            looking = false;
        }

        IEnumerator PlayAudio2Delayed(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            audio2.Play();
        }

        void LookingAt()
        {
            foreach (Outline outline in outlines)
                outline.enabled = true;
        }
    }
}
