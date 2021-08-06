using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PhoneHolding : ItemHoldable
    {
        public GameObject flashlight;
        public AudioSource audioSource;
        public Animator phoneAnim;

        bool pressedLastFrame = false;
        bool isActive = false;

        private void Start()
        {
            if (transform.parent.name == "ItemPos")
                GameController.Instance.metaPlayer.AddHoldableItem(this, true);
        }

        public override MoveData UseItem(MoveData inputData)
        {
            if (!pressedLastFrame && inputData.axisPrimary > 0)
            {
                audioSource.Play();
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

        private void OnEnable()
        {
            if (transform.parent.name == "ItemPos")
                GameController.Instance.metaPlayer.AddHoldableItem(this, true);
        }
    }
}
