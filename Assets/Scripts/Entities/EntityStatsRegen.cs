using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    [RequireComponent(typeof(EntityStats))]
    public class EntityStatsRegen : MonoBehaviour
    {
        public float hpRegenValue;
        public float hpRegenTime;
        public float hpRegenMultiplier = 1f;

        private float hpRemains = 0f;

        [Space(10)]
        public float mpRegenValue;
        public float mpRegenTime;
        public float mpRegenMultiplier = 1f;

        private EntityStats stats;

        private float mpRemains = 0f;

        void OnEnable()
        {
            stats = GetComponent<EntityStats>();
            StartCoroutine(HpRegen());
            StartCoroutine(MpRegen());
        }

        IEnumerator HpRegen()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(hpRegenTime);

                float val = hpRegenValue * hpRegenMultiplier + hpRemains;
                stats.ChangeHp(Mathf.FloorToInt(val));
                hpRemains = val % 1f;
            }
        }

        IEnumerator MpRegen()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(mpRegenTime);

                float val = mpRegenValue * mpRegenMultiplier + mpRemains;
                stats.ChangeMp(Mathf.FloorToInt(val));
                mpRemains = val % 1f;
            }
        }
    }
}
