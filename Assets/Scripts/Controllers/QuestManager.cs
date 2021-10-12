using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class QuestManager : MonoBehaviour
    {
        public Text titleText;
        public Text descriptionText;
        public Text progressText;
        public Animator completedAnim;

        [Space(20)]
        public GameObject q2Obj;
        public GameObject q3Obj;
        public GameObject q4Obj;

        private List<string> questsDone = new List<string>();
        private string currentQuestId = "";

        void Start()
        {
            StartQuest("Q1");
        }

        public bool IsQuestDone(string id)
        {
            return questsDone.Contains(id);
        }

        public void StartQuest(string id)
        {
            if (currentQuestId != "")
                EndQuest(currentQuestId);
            completedAnim.SetTrigger("NewQuest");
            completedAnim.SetBool("Completed", false);
            currentQuestId = id;
            (string, string) questTexts = questDescriptions[id];
            titleText.text = questTexts.Item1;
            descriptionText.text = questTexts.Item2;
            progressText.text = "";

            switch (id)
            {
                case "Q1":
                    break;
                case "Q2":
                    q2Obj.SetActive(true);
                    break;
                case "Q3":
                    q3Obj.SetActive(true);
                    break;
                case "Q4":
                    q4Obj.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void EndQuest(string id)
        {
            if (currentQuestId == id)
                currentQuestId = "";
            questsDone.Add(id);
            completedAnim.SetBool("Completed", true);

            switch (id)
            {
                case "Q4":
                    GameController.Instance.horrorEventManager.StartEvent("H13");
                    break;
                default:
                    break;
            }
        }

        public void SetProgressText(string text)
        {
            progressText.text = text;
        }

        private Dictionary<string, (string, string)> questDescriptions = new Dictionary<string, (string, string)> {
            {"Q1", ("The Beginning", "Speak with the king.")},
            {"Q2", ("Show Your Skills", "Kill 10 Slimy Slimes.")},
            {"Q3", ("A better weapon", "Buy a new staff.")},
            {"Q4", ("First Step", "Kill the boss in the cave.")},
            {"QEnd", ("Level Up", "Reach level 2147483648.")},
        };
    }
}
