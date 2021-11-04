using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class King : MonoBehaviour, ISaveDataObject
    {
        private static readonly Dictionary<State, string[]> texts = new Dictionary<State, string[]> {
            {State.PRE_QUEST2, new string[] {
                "Welcome, hero!",
                "I have summoned\nyou here so you\ncan help us in defeating\nthe evil demon lord!",
                "But first i want\nto see what you\nare capable of.",
                "So your first quest\nis to defeat\n10 slimes.",
                "Come back after\nyou did so."
            }},
            {State.IN_QUEST2, new string[] {"Come back after you\ndefeated 10 slimes."}},
            {State.PRE_QUEST3, new string[] {
                "Well done!",
                "Only a hero can level up this fast.",
                "As first quest, I'll have\nyou defeat a goblin lord.",
                "You'll need a better\nstaff for this,\nso go and buy one\nfrom the main street."
            }},
            {State.IN_QUEST3, new string[] {"Come back after you\nbought a new staff\nfrom the main street."}},
            {State.PRE_QUEST4, new string[] {
                "Superb!",
                "As I said, I'll have\nyou defeat the goblin lord\nhiding in the deepest\npart of the cave just\noutside of the town."
            }},
            {State.IN_QUEST4, new string[] {"Come back after you\ndefeated the goblin lord\nin the cave just outside\nof the town."}},
            {State.POST_QUEST4, new string[] {"Congratulations!\nYour next Quest will\nbe to level up."}},
            {State.IDLE, new string[] {"Come back after you\ndefeated the goblin lord\nin the cave just outside\nof the town."}}
        };

        private State currentState = State.PRE_QUEST2;
        private bool inSpeech = false;

        public string saveDataId => "king";

        private void OnTriggerEnter(Collider other)
        {
            if (!inSpeech && other.CompareTag("Player") && !other.GetComponent<PlayerController>().IsFrozen())
            {
                inSpeech = true;
                StartCoroutine(Dialogue());
            }
        }

        IEnumerator Dialogue()
        {
            if (currentState == State.IN_QUEST2 && GameController.Instance.questManager.IsQuestDone("Q2"))
                currentState = State.PRE_QUEST3;
            if (currentState == State.IN_QUEST3 && GameController.Instance.questManager.IsQuestDone("Q3"))
                currentState = State.PRE_QUEST4;
            if (currentState == State.IN_QUEST4 && GameController.Instance.questManager.IsQuestDone("Q4"))
                currentState = State.POST_QUEST4;

            yield return GameController.Instance.dialogueBubble.WriteTexts(texts[currentState]);

            if (currentState == State.PRE_QUEST2)
            {
                currentState = State.IN_QUEST2;
                GameController.Instance.questManager.StartQuest("Q2");
            }
            else if (currentState == State.PRE_QUEST3)
            {
                currentState = State.IN_QUEST3;
                GameController.Instance.questManager.StartQuest("Q3");
            }
            else if (currentState == State.PRE_QUEST4)
            {
                currentState = State.IN_QUEST4;
                GameController.Instance.questManager.StartQuest("Q4");
            }
            else if (currentState == State.POST_QUEST4)
            {
                currentState = State.IDLE;
                GameController.Instance.questManager.StartQuest("QEnd");
            }

            inSpeech = false;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("currentState", (int)currentState);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry != null)
                currentState = (State)dictEntry.GetInt("currentState", 0);
        }

        private enum State
        {
            PRE_QUEST2,
            IN_QUEST2,
            PRE_QUEST3,
            IN_QUEST3,
            PRE_QUEST4,
            IN_QUEST4,
            POST_QUEST4,
            IDLE
        }
    }
}
