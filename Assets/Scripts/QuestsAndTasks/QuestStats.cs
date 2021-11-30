using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class QuestStats : MonoBehaviour, OverallStatsObserver
    {
        public string questId;
        public string statName = "";
        public int desiredStat = 10;
        public bool addStatPreQuestToDesiredStat = false;
        public bool excludeStatPreQuest = true;

        private int statPreQuest = 0;
        private bool questDone = false;

        void Start()
        {
            GameController.Instance.overallStats.AddObserver(this, statName);
            statPreQuest = Mathf.RoundToInt(GameController.Instance.overallStats.GetStat(statName));
            if (addStatPreQuestToDesiredStat)
                desiredStat += statPreQuest;
            OnStatsUpdated();
        }

        public void OnStatsUpdated()
        {
            if (questDone)
                return;

            int currentStat = Mathf.RoundToInt(GameController.Instance.overallStats.GetStat(statName));
            if (excludeStatPreQuest)
                currentStat -= statPreQuest;
            GameController.Instance.questManager.SetProgressText(currentStat + "/" + desiredStat);

            if (currentStat >= desiredStat)
            {
                questDone = true;
                GameController.Instance.questManager.EndQuest(questId);
                GameController.Instance.overallStats.RemoveObserver(this, statName);
            }
        }
    }
}
