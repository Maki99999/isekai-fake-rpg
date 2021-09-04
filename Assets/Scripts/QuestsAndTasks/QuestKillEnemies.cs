using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class QuestKillEnemies : MonoBehaviour, OverallStatsObserver
    {
        public string questId;
        public string enemyName = "";
        public int kills = 10;

        private bool questDone = false;

        void Start()
        {
            GameController.Instance.overallStats.AddObserver(this, "Kill_" + enemyName);
            GameController.Instance.questManager.SetProgressText("0/" + kills);
        }

        public void OnStatsUpdated()
        {
            if (questDone)
                return;

            int killCount = Mathf.RoundToInt(GameController.Instance.overallStats.GetStat("Kill_" + enemyName));
            killCount = Mathf.Clamp(killCount, 0, kills);
            GameController.Instance.questManager.SetProgressText(killCount + "/10");

            if (killCount >= kills)
            {
                questDone = true;
                GameController.Instance.questManager.EndQuest(questId);
                GameController.Instance.overallStats.RemoveObserver(this, "Kill_" + enemyName);
            }
        }
    }
}
