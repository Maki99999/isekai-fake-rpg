using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class QuestStats : MonoBehaviour, OverallStatsObserver
    {
        [SerializeField] private QuestStatPart[] questParts;

        private bool questDone = false;

        void Start()
        {
            foreach (QuestStatPart questPart in questParts)
            {
                GameController.Instance.overallStats.AddObserver(this, questPart.statName);
                questPart.statPreQuest = GameController.Instance.overallStats.GetStat(questPart.statName);
                if (questPart.mode == QuestStatPart.Mode.CURRENT_TO_CURRENT_PLUS_DESIRED)
                    questPart.desiredStat += questPart.statPreQuest;
                OnStatsUpdated(questPart.statName);
            }
        }

        public void OnStatsUpdated(string category)
        {
            if (questDone)
                return;

            string progressText = "";
            int partsDone = 0;
            foreach (QuestStatPart questPart in questParts)
            {
                int currentStat = GameController.Instance.overallStats.GetStat(questPart.statName);
                if (questPart.mode == QuestStatPart.Mode.ZERO_TO_DESIRED)
                    currentStat -= questPart.statPreQuest;

                progressText += currentStat + "/" + questPart.desiredStat + "\n";

                if (!questPart.partDone && currentStat >= questPart.desiredStat)
                {
                    questPart.partDone = true;
                    partsDone++;
                }
                else if (questPart.partDone)
                    partsDone++;
            }

            GameController.Instance.questManager.SetProgressText(progressText);

            if (partsDone >= questParts.Length)
            {
                questDone = true;
                GameController.Instance.questManager.QuestAccomplished();
            }
        }

        private IEnumerator DisableNextFrame()
        {
            yield return null;
            foreach (QuestStatPart questPart in questParts)
                GameController.Instance.overallStats.RemoveObserver(this, questPart.statName);
            gameObject.SetActive(false);
        }

        [System.Serializable]
        private class QuestStatPart
        {
            public string statName = "";
            public int desiredStat = 10;
            public Mode mode = Mode.ZERO_TO_DESIRED;

            [HideInInspector] public int statPreQuest = 0;
            [HideInInspector] public bool partDone = false;

            public enum Mode
            {
                ZERO_TO_DESIRED,
                CURRENT_TO_DESIRED,
                CURRENT_TO_CURRENT_PLUS_DESIRED
            }
        }
    }
}
