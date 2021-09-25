using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T8Cracker : MonoBehaviour
    {
        public PcController pcController;

        public Collider colaCollider;
        public Collider crackerCollider;

        private IEnumerator Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'm hungry again.", "I'll get some crackers from the basement and maybe a bottle of cola." });
            if (GameController.Instance.inPcMode)
                yield return pcController.ToNonPcMode();
            colaCollider.enabled = true;
            crackerCollider.enabled = true;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        public bool BlockingPcMode()
        {
            if (GameController.Instance.metaPlayer.HasItem("Cracker"))
            {
                colaCollider.enabled = false;
                GameController.Instance.storyManager.TaskFinished();
                gameObject.SetActive(false);
                return false;
            }
            else if (GameController.Instance.metaPlayer.HasItem("Cola"))
            {
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'm hungry, I'll get some crackers from the basement." });
                return true;
            }
            else
            {
                GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I'll get some crackers from the basement and maybe a bottle of cola." });
                return true;
            }
        }
    }
}
