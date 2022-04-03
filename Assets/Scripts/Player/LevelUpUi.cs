using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class LevelUpUi : MonoBehaviour
    {
        public PlayerStats playerStats;

        public Animator animator;
        public Text tPoints;

        [Space(10)]
        public Text tHp;
        public Text tHpRegen;
        public Text tMp;
        public Text tMpRegen;

        [Space(10)]
        public GameObject bHp;
        public GameObject bHpRegen;
        public GameObject bMp;
        public GameObject bMpRegen;

        public void SetValues(PlayerStatPoints statPoints)
        {
            tHp.text = statPoints.hp.ToString();
            tHpRegen.text = statPoints.hpRegen.ToString();
            tMp.text = statPoints.mp.ToString();
            tMpRegen.text = statPoints.mpRegen.ToString();
            tPoints.text = statPoints.freePoints.ToString();

            SetButtonVisibility(statPoints.freePoints > 0);
            animator.SetBool("LeveledUp", statPoints.freePoints > 0);
        }

        public void PlayAnim()
        {
            animator.SetTrigger("LeveledUpJustNow");
        }

        private void SetButtonVisibility(bool visible)
        {
            bHp.SetActive(visible);
            bHpRegen.SetActive(visible);
            bMp.SetActive(visible);
            bMpRegen.SetActive(visible);
        }

        public void OkButton()
        {
            playerStats.ToggleLevelMenu();
        }

        public void SetVisibility(bool visible)
        {
            animator.SetBool("Screen", visible);
        }

        public void AddHpStat() { SetValues(playerStats.UseFreeStatPoint(StatPointName.Hp)); }
        public void AddHpRegenStat() { SetValues(playerStats.UseFreeStatPoint(StatPointName.HpRegen)); }
        public void AddMpStat() { SetValues(playerStats.UseFreeStatPoint(StatPointName.Mp)); }
        public void AddMpRegenStat() { SetValues(playerStats.UseFreeStatPoint(StatPointName.MpRegen)); }
    }
}
