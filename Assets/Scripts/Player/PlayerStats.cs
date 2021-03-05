using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PlayerStats : EntityStats
    {
        [Space(10)]
        public PlayerController player;
        public LevelUpUi levelUpUi;
        public EntityStatsRegen statsRegen;
        public PlayerStatPoints statPoints;

        private int coins;

        private int levelInt = 1;
        private int currentXp = 0;
        private int lastNeededXp = 0;
        private int nextNeededXp = 7;

        private bool inLevelMenu = false;
        private bool pressingButton = false;

        void Start()
        {
            entityStats = GameController.Instance.entityStats;
            uiCam = GameController.Instance.gamePlayer.cam;

            statsUi = statsUiPrefab;
            entityStatsUi = statsUi.GetComponentInChildren<PlayerStatsUi>();
            entityStatsUi.SetMaxHp(maxHp, maxHp);
            entityStatsUi.SetMaxMp(maxMp, maxMp);
            if (isInvincible || maxHp <= 0)
                entityStatsUi.SetHpActive(false);
            if (maxMp <= 0)
                entityStatsUi.SetMpActive(false);

            entityStatsUi.SetText("Level " + Mathf.FloorToInt(level));
            entityStatsUi.zValue = Mathf.NegativeInfinity;

            statPoints.freePoints = 101; //TODO: DEBUG
            levelUpUi.SetValues(statPoints);
        }

        void Update()
        {
            if (GameController.Instance.inPcMode && !PauseManager.isPaused().Value && InputSettings.PressingLevel() && !pressingButton)
            {
                toggleLevelMenu();
            }
            pressingButton = InputSettings.PressingLevel();
        }

        public void toggleLevelMenu()
        {
            inLevelMenu = !inLevelMenu;
            if (inLevelMenu)
                GameController.Instance.UnlockMouse();
            else
                GameController.Instance.LockMouse();
            player.canControl = !inLevelMenu;
            levelUpUi.SetVisibility(inLevelMenu);
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
            level = levelInt + (((float)currentXp - lastNeededXp) / (nextNeededXp - lastNeededXp));
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

            statPoints.freePoints++;
            levelUpUi.SetValues(statPoints);
        }

        public PlayerStatPoints UseFreeStatPoint(StatPointName statName)
        {
            if (statPoints.freePoints > 0)
            {
                statPoints.ChangePoints(statName, 1);
                statPoints.ChangePoints(StatPointName.FreePoints, -1);
                PointsChanged();
            }
            return statPoints;
        }

        private void PointsChanged()
        {
            int oldHp = maxHp;
            int oldMp = maxMp;
            maxHp = 150 + (statPoints.hp * 10);
            maxMp = 65 + (statPoints.mp * 7);
            statsRegen.hpRegenMultiplier = 1f + (statPoints.hpRegen * 0.2f);
            statsRegen.mpRegenMultiplier = 1f + (statPoints.mpRegen * 0.3f);

            entityStatsUi.SetMaxHp(maxHp, -1);
            entityStatsUi.SetMaxMp(maxMp, -1);
            ChangeHp(maxHp - oldHp);
            ChangeMp(maxMp - oldMp);
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

    [System.Serializable]
    public class PlayerStatPoints
    {
        public int hp = 5;
        public int hpRegen = 5;
        public int mp = 5;
        public int mpRegen = 5;

        public int freePoints;

        public void ChangePoints(StatPointName statName, int value)
        {
            switch (statName)
            {
                case StatPointName.Hp:
                    hp += value;
                    break;
                case StatPointName.HpRegen:
                    hpRegen += value;
                    break;
                case StatPointName.Mp:
                    mp += value;
                    break;
                case StatPointName.MpRegen:
                    mpRegen += value;
                    break;
                case StatPointName.FreePoints:
                    freePoints += value;
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public enum StatPointName
    {
        Hp,
        HpRegen,
        Mp,
        MpRegen,
        FreePoints
    }
}
