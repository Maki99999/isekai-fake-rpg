using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T14MissingPhone : MonoBehaviour, Task
    {
        public PcController pcController;

        IEnumerator Start()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "Time...", "Where's my phone?"});
            StartCoroutine(pcController.ToNonPcMode());
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'll look for it." });
            GameController.Instance.horrorEventManager.StartEvent("H9");
            GameController.Instance.horrorEventManager.StartEvent("H16");
            GameController.Instance.playerEventManager.FreezePlayers(false);
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm looking for my phone." });
            return true;
        }

        public void SkipTask()
        { }
    }
}
