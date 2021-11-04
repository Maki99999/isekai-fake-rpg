using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class QuestManager : MonoBehaviour, ISaveDataObject
    {
        public Text titleText;
        public Text descriptionText;
        public Text progressText;
        public Animator completedAnim;

        [Space(20)]
        public GameObject q2Obj;
        public GameObject q3Obj;
        public GameObject q4Obj;

        public string saveDataId => "questManager";

        private HashSet<string> finishedQuests = new HashSet<string>();
        private string currentQuestId = "";

        public bool IsQuestDone(string id)
        {
            return finishedQuests.Contains(id);
        }

        public void StartQuest(string id)
        {
            if (currentQuestId != "")
                EndQuest(currentQuestId);

            if (id == "")
                return;

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
            finishedQuests.Add(id);
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

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("finishedQuests", new List<string>(finishedQuests));
            entry.Add("currentQuestId", currentQuestId);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
            {
                StartQuest("Q1");
                return;
            }
            finishedQuests = new HashSet<string>(dictEntry.GetList("finishedQuests", new List<string>()));
            string quest = dictEntry.GetString("currentQuestId", "");
            if (quest == "" && finishedQuests.Count < questDescriptions.Count)
            {
                string lastQuest = "Q1";
                foreach (string finishedQuest in finishedQuests)
                    if (string.Compare(finishedQuest, lastQuest) > 0)
                        lastQuest = finishedQuest;

                (string, string) questTexts = questDescriptions[lastQuest];
                titleText.text = questTexts.Item1;
                descriptionText.text = questTexts.Item2;
                progressText.text = "";
                completedAnim.SetBool("Completed", true);
            }
            StartQuest(quest);
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
