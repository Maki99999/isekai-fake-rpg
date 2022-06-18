using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H8Mirror : MonoBehaviour
    {
        public Camera reflectionCamera;
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
            origPos = ghost.position;
            origRot = ghost.rotation;
            ghost.position = origPos - Vector3.down * 100;

            RenderTexture.active = reflectionCamera.targetTexture;
            GL.Clear(true, true, Color.black);
        }

        void Update()
        {
            if (playerNearby)
            {
                if (!reflectionCamera.enabled)
                    reflectionCamera.enabled = true;

                Vector3 toPlayer = GameController.Instance.metaPlayer.eyeHeightTransform.position - transform.parent.position;
                float angle = Vector3.SignedAngle(toPlayer, transform.parent.forward, transform.parent.up);
                reflectionCamera.transform.eulerAngles = new Vector3(0f, 90f + angle, 0f);
            }
            else if (!playerNearby && reflectionCamera.enabled)
                reflectionCamera.enabled = false;
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
