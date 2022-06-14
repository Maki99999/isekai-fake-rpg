using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H24Stalker : MonoBehaviour
    {
        private static bool stalkerVisible = false;

        public Animator stalkerAnim;
        new public Light light;
        public Light torchLight;
        public AudioSource hideSfx;

        private bool thisStalkerVisible = false;
        private bool playerInTrigger = false;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(Random.Range(20f, 400f));
            while (stalkerVisible || playerInTrigger)
                yield return new WaitForSeconds(Random.Range(10f, 100f));

            Show();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInTrigger = true;
                if (thisStalkerVisible)
                    StartCoroutine(Hide());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                playerInTrigger = false;
        }

        private void Show()
        {
            stalkerVisible = true;
            thisStalkerVisible = true;
            stalkerAnim.gameObject.SetActive(true);
            light.enabled = true;
        }

        private IEnumerator Hide()
        {
            stalkerVisible = false;
            thisStalkerVisible = false;
            stalkerAnim.SetTrigger("Hide");
            hideSfx.Play();

            float lightIntensityStart = light.intensity;
            float torchLightIntensityStart = torchLight.intensity;

            for (float f = 0; f < 1f; f += Time.deltaTime)
            {
                light.intensity = Mathf.SmoothStep(lightIntensityStart, 0f, f);
                torchLight.intensity = Mathf.SmoothStep(torchLightIntensityStart, 0f, f);
                yield return null;
            }

            light.enabled = false;
            stalkerAnim.gameObject.SetActive(false);
        }
    }
}
