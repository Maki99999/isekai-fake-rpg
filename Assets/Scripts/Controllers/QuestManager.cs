using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Default
{
    public class QuestManager : MonoBehaviour, ISaveDataObject
    {
        public Text titleText;
        public Text descriptionText;
        public Text progressText;
        public Animator completedAnim;

        [SerializeField] private QuestAndEvent[] questStartEvents;
        private Dictionary<string, UnityEvent> questStartEventsDict;

        private string startQuestId = "Q1";
        public string currentQuestId { get; private set; } = "Q1";
        public QuestState currentQuestState { get; private set; } = QuestState.PRE_ACCEPTED;

        public GameObject fakeHero;

        public string saveDataId => "questManager";

        private void Awake()
        {
            questStartEventsDict = new Dictionary<string, UnityEvent>();
            foreach (QuestAndEvent qae in questStartEvents)
                questStartEventsDict.Add(qae.questId, qae.unityEvent);
        }

        public bool IsQuestDone(string id)
        {
            return System.Array.IndexOf(questOrder, id) < System.Array.IndexOf(questOrder, currentQuestId) - 1;
        }

        public bool IsQuestActiveAndAccepted(string id)
        {
            return currentQuestId == id && currentQuestState == QuestState.PRE_ACCOMPLISHED;
        }

        private void InitQuest(string id)
        {
            currentQuestId = id;
            currentQuestState = QuestState.PRE_ACCEPTED;

            if (id == "Q4")
                GameController.Instance.horrorEventManager.StartEvent("H21");
        }

        public void QuestAccepted()
        {
            currentQuestState = QuestState.PRE_ACCOMPLISHED;

            if (questStartEventsDict.ContainsKey(currentQuestId))
                questStartEventsDict[currentQuestId].Invoke();

            completedAnim.SetBool("Completed", false);
            completedAnim.SetTrigger("NewQuest");
            (string, string) questTexts = questDescriptions[currentQuestId];
            titleText.text = questTexts.Item1;
            descriptionText.text = questTexts.Item2;
            progressText.text = "";
        }

        public void QuestAccomplished()
        {
            currentQuestState = QuestState.PRE_CONFIRMED;
            completedAnim.SetBool("Completed", true);

            if (currentQuestId == "Q4")
            {
                GameController.Instance.horrorEventManager.StartEvent("H13");
                fakeHero.SetActive(true);
            }
        }

        public void QuestConfirmed()
        {
            InitQuest(questOrder[System.Array.IndexOf(questOrder, currentQuestId) + 1]);
        }

        public void SetProgressText(string text)
        {
            progressText.text = text;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("currentQuestId", currentQuestId);
            entry.Add("currentQuestState", (int)currentQuestState);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
            {
                if (startQuestId == questOrder[0])
                {
                    InitQuest(startQuestId);
                    QuestAccepted();
                }
                else
                {
                    InitQuest(questOrder[System.Array.IndexOf(questOrder, currentQuestId) - 1]);
                    QuestAccepted();
                    QuestAccomplished();
                }
                return;
            }
            string quest = dictEntry.GetString("currentQuestId", startQuestId);
            currentQuestState = (QuestState)dictEntry.GetInt("currentQuestState", (int)currentQuestState);
            InitQuest(quest);
            if (currentQuestState == QuestState.PRE_ACCOMPLISHED || currentQuestState == QuestState.PRE_CONFIRMED)
                QuestAccepted();
            if (currentQuestState == QuestState.PRE_CONFIRMED)
                QuestAccomplished();
        }

        public enum QuestState
        {
            PRE_ACCEPTED,
            PRE_ACCOMPLISHED,
            PRE_CONFIRMED
        }

        [System.Serializable]
        private class QuestAndEvent
        {
            public string questId;
            public UnityEvent unityEvent;
        }

        private string[] questOrder = new string[] { "Q1", "Q2", "Q3", "Q6", "Q7", "Q8", "Q9", "Q10", "Q11", "Q5", "Q4", "Q12", "Q13", "QEnd" };

        private Dictionary<string, (string, string)> questDescriptions = new Dictionary<string, (string, string)> {
            {"Q1", ("The Beginning", "Speak with the king.")},
            {"Q2", ("Show Your Skills", "Kill 10 Slimy Slimes.")},
            {"Q3", ("Shopping", "Buy a new staff.")},
            {"Q4", ("Be A Hero", "Kill the boss in the cave.")},
            {"Q5", ("Too Weak", "Level up 10 times.")},
            {"Q6", ("Pest Control", "Kill 5 Spiders and 5 Rats.")},
            {"Q7", ("Errands", "Find out what exactly is blocking the bridge.")},
            {"Q8", ("Broken Cart", "Find someone who can fix the cart.")},
            {"Q9", ("Bad News", "Report back to the king that a goblin was sighted.")},
            {"Q10", ("Scouting", "Find out if there really are goblins in the cave.")},
            {"Q11", ("Shopping", "Buy a second tier staff and second tier armor.")},
            {"Q12", ("Who Is The Real Deal?", "Duel with the fake hero.")},
            {"Q13", ("The Ultimate Scroll", "Find the first clue to locate the scroll.")},
            {"QEnd", ("Still Too Weak", "Reach level 2147483648.")},
        };
    }
}
