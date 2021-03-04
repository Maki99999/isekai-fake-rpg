using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class PlayerStats : EntityStats
    {
        [Space(10)]
        public PlayerController player;

        private int coins;

        private int levelInt = 1;
        private int currentXp = 0;
        private int lastNeededXp = 0;
        private int nextNeededXp = 7;

        void Start()
        {
            entityStats = GameController.Instance.entityStats;
            uiCam = GameController.Instance.gamePlayer.cam;

            statsUi = Instantiate(statsUiPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), entityStats);
            entityStatsUi = statsUi.GetComponentInChildren<PlayerStatsUi>();
            entityStatsUi.SetMaxHp(maxHp);
            entityStatsUi.SetMaxMp(maxMp);
            if (isInvincible || maxHp <= 0)
                entityStatsUi.SetHpActive(false);
            if (maxMp <= 0)
                entityStatsUi.SetMpActive(false);


            rectTransform = statsUi.GetComponent<RectTransform>();
            rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
            rectTransform.localPosition = Vector3.zero;

            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(15f, -10f);

            entityStatsUi.SetText("Level " + Mathf.FloorToInt(level));
            entityStatsUi.zValue = Mathf.NegativeInfinity;
        }

        public void ShakeMp()
        {
            ((PlayerStatsUi)entityStatsUi).ShakeMp();
        }

        void ShowLevel()
        {
            entityStatsUi.SetText("Level " + Mathf.FloorToInt(level));
            ((PlayerStatsUi)entityStatsUi).SetSublevel(level % 1f);
        }

        public void AddXp(int xp)
        {
            currentXp += xp;
            if (currentXp >= nextNeededXp)
                LevelUp();
            level = levelInt + (((float) currentXp - lastNeededXp) / (nextNeededXp - lastNeededXp));
            ShowLevel();
        }

        //For Saving/Loading
        public void SetXp(int xp)
        {
            currentXp = xp;
            //level = NextLevelRequirementInversed...
        }

        public void LevelUp()
        {
            levelInt++;
            lastNeededXp = nextNeededXp;
            nextNeededXp = NextLevelRequirement(levelInt);

            //TODO: Rewards
        }

        private static int NextLevelRequirement(int currlevel)
        {
            return Mathf.RoundToInt(3.9391f * Mathf.Exp(0.6967f * currlevel));
        }

        private static int NextLevelRequirementInversed(int xp)
        {
            return Mathf.FloorToInt(Mathf.Log(xp / 3.9391f) / (0.6967f));
        }

        public bool ChangeCoins(int changeValue)
        {
            if (coins + changeValue < 0)
                return false;

            coins += changeValue;
            return true;
        }
    }
}
