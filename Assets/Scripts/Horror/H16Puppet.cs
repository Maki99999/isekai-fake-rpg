using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H16Puppet : MonoBehaviour
    {
        public GameObject puppetRagdollPrefab;
        public float minBreakTime = 1f;
        public float maxBreakTime = 24f;

        public DestroyWhenNotLooking currentRagdoll;
        public AudioSource ragdollSfx;

        void Update()
        {
            Vector3 playerPos = GameController.Instance.metaPlayer.cam.transform.position;
            transform.position = playerPos
                                 - GameController.Instance.metaPlayer.transform.forward * 2f
                                 - Vector3.up * 0.3f;
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
        }

        public void OnEnable()
        {
            StartCoroutine(BreakRandomly());
        }

        IEnumerator BreakRandomly()
        {
            yield return new WaitForSeconds(Random.Range(minBreakTime, maxBreakTime));

            ragdollSfx.Play();
            currentRagdoll.enabled = true;
            currentRagdoll = Instantiate(puppetRagdollPrefab, transform.position, transform.rotation).GetComponent<DestroyWhenNotLooking>();

            yield return BreakRandomly();
        }
    }
}
