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

        bool playerNearby = false;
        Vector3 origPos;
        Quaternion origRot;

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
            ghostAnim.SetTrigger("Show");
        }

        public void HideGhost()
        {
            StartCoroutine(Hide());
        }

        IEnumerator Hide()
        {
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
