using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class Repairman : MonoBehaviour
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
            GameController.Instance.dialogueBubble.StopAllCoroutines();
            GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
            if (currentQuestId == "Q8" && currentQuestState == QuestManager.QuestState.PRE_ACCOMPLISHED)
            {
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "I need to repair something?\nLeave it to me!"
                });
                GameController.Instance.questManager.QuestAccomplished();
            }
            else
            {
                yield return GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Need something repaired?"
                });
            }
            inSpeech = false;
        }

        public void ShoutToPlayer()
        {
            if (GameController.Instance.questManager.IsQuestActiveAndAccepted("Q8"))
            {
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                StartCoroutine(GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Repairing stuff!"
                }));
            }
        }
    }
}
