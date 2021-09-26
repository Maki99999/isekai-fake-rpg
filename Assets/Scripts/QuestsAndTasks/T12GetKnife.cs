using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T12GetKnife : MonoBehaviour
    {
        public Collider knifeCollider;

        void Start()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm not feeling safe. I'll grab a knife." });
            knifeCollider.enabled = true;
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Something's here. I'll get a knife." });
            return true;
        }
    }
}
