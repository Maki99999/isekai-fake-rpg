using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T12GetKnife : MonoBehaviour, Task
    {
        public Collider knifeCollider;

        IEnumerator Start()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm not feeling safe. I'll grab a knife." });
            knifeCollider.enabled = true;

            yield return new WaitForSeconds(16f);
            GameController.Instance.horrorEventManager.StartEvent("H1");
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Something's here. I'll get a knife." });
            return true;
        }

        public void SkipTask() { }
    }
}
