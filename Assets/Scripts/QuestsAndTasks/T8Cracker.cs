using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T8Cracker : MonoBehaviour, Task
    {
        public PcController pcController;

        public Collider colaCollider;
        public Collider crackerCollider;

        private IEnumerator Start()
        {
            GameController.Instance.playerEventManager.FreezePlayers(true, true);
            yield return GameController.Instance.dialogue.StartDialogue(new List<string>() { "I'm hungry again.", "I'll get some crackers from the basement and maybe a bottle of cola." });
            if (GameController.Instance.inPcMode)
                yield return pcController.ToNonPcMode();
            colaCollider.enabled = true;
            crackerCollider.enabled = true;
            GameController.Instance.playerEventManager.FreezePlayers(false);
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

        public void SkipTask()
        {
            GameController.Instance.metaPlayer.AddItem(colaCollider.GetComponent<ItemHoldable>(), false, false);
            GameController.Instance.metaPlayer.AddItem(crackerCollider.GetComponent<ItemHoldable>(), false, false);
            colaCollider.gameObject.SetActive(false);
            crackerCollider.gameObject.SetActive(false);
        }
    }
}
