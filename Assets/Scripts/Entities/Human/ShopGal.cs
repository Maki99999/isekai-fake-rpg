using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ShopGal : MonoBehaviour
    {
        public Transform uiFollowPos;
        public GameObject shopItems;

        private void OnTriggerEnter(Collider other)
        {
            if (enabled && other.CompareTag("Player") && (GameController.Instance.questManager.IsQuestDone("Q3")
                    || GameController.Instance.questManager.IsQuestActiveAndAccepted("Q3")))
            {
                shopItems.SetActive(true);
                GameController.Instance.dialogueBubble.followPosition = uiFollowPos;
                StartCoroutine(GameController.Instance.dialogueBubble.WriteTexts(new string[] {
                    "Selling stuff!"
                }));
            }
        }
    }
}
