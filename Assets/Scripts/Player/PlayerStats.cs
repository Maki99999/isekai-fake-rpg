using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PlayerStats : EntityStats, ISaveDataObject
    {
        [Space(10)]
        public PlayerController player;
        public LevelUpUi levelUpUi;
        public EntityStatsRegen statsRegen;
        public PlayerStatPoints statPoints;

        [Space(10)]
        public AudioSource levelUpFx;
        public AudioSource killedSthFx;

        [Space(10)]
        public Transform statItemsTransform;
        public Armor statItems;
        private bool hasStatItem = false;

        [SerializeField]
        private int coins = 0;

        private int levelInt = 1;
        private int currentXp = 0;
        private int lastNeededXp = 0;
        private int nextNeededXp = 7;

        private bool inLevelMenu = false;
        private bool pressingButton = false;

        public string saveDataId => "PlayerStats" + gameObject.name;

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
            ((PlayerStatsUi)entityStatsUi).SetCoins(coins);

            entityStatsUi.SetText("Level " + Mathf.FloorToInt(level));
            entityStatsUi.zValue = Mathf.NegativeInfinity;

            levelUpUi.SetValues(statPoints);

            GameController.Instance.overallStats.SetStat(levelInt, "PlayerLevel");
        }

        void Update()
        {
            if (GameController.Instance.inPcMode && !PauseManager.isPaused().Value && InputSettings.PressingLevel() && !pressingButton)
            {
                ToggleLevelMenu();
            }
            pressingButton = InputSettings.PressingLevel();
        }

        public void ToggleLevelMenu()
        {
            ShowLevelMenu(!inLevelMenu);
        }

        public void ShowLevelMenu(bool show)
        {
            if (show == inLevelMenu)
                return;
            inLevelMenu = show;
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
            killedSthFx.Play();
            currentXp += xp;
            if (currentXp >= nextNeededXp)
                LevelUp();
            level = levelInt + (((float)currentXp - lastNeededXp) / (nextNeededXp - lastNeededXp));
            ShowLevel();
        }

        public void LevelUp()
        {
            while (currentXp >= nextNeededXp)
            {
                levelInt++;
                lastNeededXp = nextNeededXp;
                nextNeededXp = NextLevelRequirement(levelInt);

                statPoints.freePoints++;
            }
            GameController.Instance.overallStats.SetStat(levelInt, "PlayerLevel");

            levelUpUi.PlayAnim();
            levelUpUi.SetValues(statPoints);
            levelUpFx.Play();
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

        public void AddOrReplaceStatItem(Armor item)   //Maybe with slots later on, for now there is only one slot
        {
            if (statItems != null)
                statPoints.ChangePoints(StatPointName.Hp, -statItems.hpStatPoints);

            hasStatItem = true;
            statItems = item;
            statPoints.ChangePoints(StatPointName.Hp, statItems.hpStatPoints);
            item.transform.SetParent(statItemsTransform);
            item.transform.localPosition = -Vector3.forward;

            PointsChanged();
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

            levelUpUi.SetValues(statPoints);
        }

        private static int NextLevelRequirement(int currlevel)
        {
            //1 -> 8, 6 -> 30, 40 -> 300
            return Mathf.RoundToInt((293.371f * Mathf.Exp(0.0729913f * currlevel)) - 307.585f);
        }

        public bool ChangeCoins(int changeValue)
        {
            if (coins + changeValue < 0)
                return false;

            coins += changeValue;
            ((PlayerStatsUi)entityStatsUi).SetCoins(coins);
            return true;
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();

            entry.Add("maxHp", maxHp);
            entry.Add("maxMp", maxMp);
            entry.Add("level", level);
            entry.Add("statsRegen.hpRegenMultiplier", statsRegen.hpRegenMultiplier);
            entry.Add("statsRegen.mpRegenMultiplier", statsRegen.mpRegenMultiplier);
            if (hasStatItem)
            {
                entry.Add("statPoints.hp", statPoints.hp - statItems.hpStatPoints);
            }
            else
                entry.Add("statPoints.hp", statPoints.hp);
            entry.Add("statPoints.hpRegen", statPoints.hpRegen);
            entry.Add("statPoints.mp", statPoints.mp);
            entry.Add("statPoints.mpRegen", statPoints.mpRegen);
            entry.Add("statPoints.freePoints", statPoints.freePoints);
            entry.Add("coins", coins);
            entry.Add("levelInt", levelInt);
            entry.Add("currentXp", currentXp);
            entry.Add("lastNeededXp", lastNeededXp);
            entry.Add("nextNeededXp", nextNeededXp);

            return entry;
        }

        public void Load(SaveDataEntry dataEntry)
        {
            if (dataEntry == null)
                return;
            StartCoroutine(LoadNextFrame(dataEntry));
        }

        private IEnumerator LoadNextFrame(SaveDataEntry entry)
        {
            yield return null;
            maxHp = entry.GetInt("maxHp", maxHp);
            hp = maxHp;
            maxMp = entry.GetInt("maxMp", maxMp);
            mp = maxMp;
            level = entry.GetFloat("level", level);
            statsRegen.hpRegenMultiplier = entry.GetFloat("statsRegen.hpRegenMultiplier", statsRegen.hpRegenMultiplier);
            statsRegen.mpRegenMultiplier = entry.GetFloat("statsRegen.mpRegenMultiplier", statsRegen.mpRegenMultiplier);
            if (hasStatItem)
            {
                statPoints.hp = statPoints.hp - 5 + entry.GetInt("statPoints.hp", statPoints.hp);
            }
            else
                statPoints.hp = entry.GetInt("statPoints.hp", statPoints.hp);
            statPoints.hpRegen = entry.GetInt("statPoints.hpRegen", statPoints.hpRegen);
            statPoints.mp = entry.GetInt("statPoints.mp", statPoints.mp);
            statPoints.mpRegen = entry.GetInt("statPoints.mpRegen", statPoints.mpRegen);
            statPoints.freePoints = entry.GetInt("statPoints.freePoints", statPoints.freePoints);
            coins = entry.GetInt("coins", coins);
            levelInt = entry.GetInt("levelInt", levelInt);
            currentXp = entry.GetInt("currentXp", currentXp);
            lastNeededXp = entry.GetInt("lastNeededXp", lastNeededXp);
            nextNeededXp = entry.GetInt("nextNeededXp", nextNeededXp);

            entityStatsUi.SetMaxHp(maxHp, -1);
            entityStatsUi.SetMaxMp(maxMp, -1);
            entityStatsUi.SetText("Level " + Mathf.FloorToInt(level));
            levelUpUi.SetValues(statPoints);
            ((PlayerStatsUi)entityStatsUi).SetCoins(coins);
            ((PlayerStatsUi)entityStatsUi).SetSublevel(level % 1f);
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
