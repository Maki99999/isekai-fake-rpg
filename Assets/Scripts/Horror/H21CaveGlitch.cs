using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H21CaveGlitch : MonoBehaviour
    {
        [SerializeField] private Animator glitchMonster;
        [SerializeField] private MonsterGlitchEffectReceiver glitchEffectReceiver;

        private bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.CompareTag("Player"))
            {
                triggered = true;
                StartCoroutine(ShowGlitch());
            }
        }

        private IEnumerator ShowGlitch()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 25f);
            foreach (Collider collider in colliders)
            {
                if (collider.name == "Hitbox")
                    Destroy(collider.transform.parent.gameObject);
            }
            glitchMonster.SetBool("Show", true);
            yield return GameController.Instance.playerEventManager.LookAt(true, glitchMonster.transform.position, 0.2f);
            glitchEffectReceiver.enabled = true;
            yield return new WaitForSeconds(2.5f);
            yield return TransformOperations.MoveTo
            (
                glitchMonster.transform,
                GameController.Instance.gamePlayer.transform.position,
                glitchMonster.transform.rotation,
                .2f
            );
            glitchEffectReceiver.enabled = false;
            glitchMonster.SetBool("Show", false);
            yield return new WaitForSeconds(1f);
            GameController.Instance.playerEventManager.FreezePlayer(true, true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Weird Glitch..." });
            GameController.Instance.playerEventManager.FreezePlayer(true, false);
            gameObject.SetActive(false);
        }
    }
}
