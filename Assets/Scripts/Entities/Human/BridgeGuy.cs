using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class BridgeGuy : MonoBehaviour
    {
        public Transform uiFollowPos;
        private bool inSpeech = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!inSpeech && other.CompareTag("Player") && !other.GetComponent<PlayerController>().IsFrozen())
            {
                StartCoroutine(Dialogue());
            }
        }

        private IEnumerator Dialogue()
        {
            inSpeech = true;
            string currentQuestId = GameController.Instance.questManager.currentQuestId;
            QuestManager.QuestState currentQuestState = GameController.Instance.questManager.currentQuestState;

            if (currentQuestId == "Q7")
            {
                GameController.Instance.questManager.QuestAccomplished();
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Aww man, my poor cart.",
                    "Hey you, can you help me?",
                    "Awesome.\nWould be really cool if you could\nfind someone in the city\nthat can repair something,\nlike a repairman or something."
                });
                GameController.Instance.questManager.QuestConfirmed();
                GameController.Instance.questManager.QuestAccepted();
            }
            else if (currentQuestId == "Q8" && currentQuestState == QuestManager.QuestState.PRE_ACCOMPLISHED)
            {
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Please find me a craftsman, my friend.",
                });
            }
            else if (currentQuestId == "Q8" && currentQuestState == QuestManager.QuestState.PRE_CONFIRMED)
            {
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Thanks. Very Cool.",
                    "Hopefully I can leave soon.\nI don't want to spend the\nnight with the goblins in the cave.",
                    "Oh, you haven't heard\nof the goblins?\nAre they new?",
                    "Hopefully the king already\nknows of the goblins!"
                });
                GameController.Instance.questManager.QuestConfirmed();
                GameController.Instance.questManager.QuestAccepted();
            }
            else if (currentQuestId == "Q9")
            {
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Hopefully the king already\nknows of the goblins!"
                });
            }
            inSpeech = false;
        }
    }
}
