using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H23Disappear : MonoBehaviour
    {
        public GameObject disappearObj;
        public AudioSource chirpNoise;
        public MonsterGlitchEffectReceiver glitchEffectReceiver;

        private void OnTriggerEnter(Collider other)
        {
            if (enabled && other.CompareTag("Player"))
            {
                enabled = false;
                StartCoroutine(EffectAndDisappear());
            }
        }

        private IEnumerator EffectAndDisappear()
        {
            chirpNoise.Play();
            disappearObj.SetActive(false);

            glitchEffectReceiver.enabled = true;
            glitchEffectReceiver.desiredPercent = 1;
            yield return new WaitForSeconds(0.5f);
            glitchEffectReceiver.desiredPercent = 0;
            glitchEffectReceiver.enabled = false;
        }
    }
}
