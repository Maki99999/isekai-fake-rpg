using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H8Mirror : MonoBehaviour
    {
        public ReflectionProbe reflectionProbe;
        public AudioSource audioSource;
        public Animator ghostAnim;
        public Transform ghost;
        public LayerMask ghostLookAtLayer;

        private bool playerNearby = false;
        private bool ghostShowing = false;
        private Vector3 origPos;
        private Quaternion origRot;

        void Start()
        {
            reflectionProbe.RenderProbe();
            origPos = ghost.position;
            origRot = ghost.rotation;
            ghost.position = origPos - Vector3.down * 100;
        }

        void Update()
        {
            if (playerNearby)
                reflectionProbe.RenderProbe();
        }

        public void ShowGhost()
        {
            ghost.position = origPos;
            ghost.rotation = origRot;
            ghostShowing = true;
            ghostAnim.SetTrigger("Show");
            StartCoroutine(HideWhenLooking());
        }

        public void HideGhost()
        {
            StartCoroutine(Hide());
        }

        IEnumerator HideWhenLooking()
        {
            Transform playerTransform = GameController.Instance.metaPlayer.cam.transform;
            RaycastHit hit;
            while (ghostShowing)
            {
                if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, 2.5f, ghostLookAtLayer))
                {
                    if (hit.collider.gameObject.CompareTag("LookAtTrigger"))
                        break;
                }
                yield return new WaitForSeconds(0.5f);
            }

            yield return Hide();
        }

        IEnumerator Hide()
        {
            ghostShowing = false;
            audioSource.Play();
            ghostAnim.SetTrigger("Hide");
            yield return new WaitForSeconds(1f);
            ghost.position = origPos - Vector3.down * 100;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                playerNearby = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerNearby = false;
        }
    }
}
