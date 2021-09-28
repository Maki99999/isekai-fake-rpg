using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T14MissingPhone : MonoBehaviour
    {
        void Start()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "Time...", "Where's my phone?", "I'll look for it." });
            GameController.Instance.horrorEventManager.StartEvent("H9");
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm looking for my phone." });
            return true;
        }
    }
}
