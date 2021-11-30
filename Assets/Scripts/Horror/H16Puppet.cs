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

        void Update()
        {
            transform.position = GameController.Instance.metaPlayer.transform.position
                                 - GameController.Instance.metaPlayer.transform.forward * 2f
                                 + Vector3.up * 0.3f;
            transform.LookAt(GameController.Instance.metaPlayer.cam.transform.position);
        }

        public void OnEnable()
        {
            StartCoroutine(BreakRandomly());
        }

        IEnumerator BreakRandomly()
        {
            yield return new WaitForSeconds(Random.Range(minBreakTime, maxBreakTime));
            currentRagdoll.enabled = true;

            currentRagdoll = Instantiate(puppetRagdollPrefab, transform.position, transform.rotation).GetComponent<DestroyWhenNotLooking>();

            yield return BreakRandomly();
        }
    }
}
