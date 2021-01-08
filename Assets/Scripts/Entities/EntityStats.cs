using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class EntityStats : MonoBehaviour
    {
        public string displayName;
        public int hp;
        public int maxHp;
        public int mp;
        public int maxMp;
        public int level;
        public bool isInvincible = false;
        public float height;

        private Canvas canvas;
        private Camera uiCam;
        private GameObject statsUi;
        private RectTransform rectTransform;
        private EnemyStatsUi enemyStatsUi;
        private Transform entityStats;

        public GameObject statsUiPrefab;

        [HideInInspector] public List<EntityStatsObserver> entityStatsObservers = new List<EntityStatsObserver>();

        private void Start()
        {
            entityStats = GameController.Instance.entityStats;
            canvas = entityStats.GetComponentInParent<Canvas>();
            uiCam = canvas.worldCamera;

            statsUi = Instantiate(statsUiPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), entityStats);
            enemyStatsUi = statsUi.GetComponentInChildren<EnemyStatsUi>();
            enemyStatsUi.SetMaxHp(maxHp);
            enemyStatsUi.SetMaxMp(maxMp);
            if (isInvincible || maxHp <= 0)
                enemyStatsUi.SetHpActive(false);
            if (maxMp <= 0)
                enemyStatsUi.SetMpActive(false);


            rectTransform = statsUi.GetComponent<RectTransform>();
            rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.pivot = new Vector2(0.5f, 0f);
            enemyStatsUi.SetText(name + " Lvl. " + level);

            if (CompareTag("Player"))
            {
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = new Vector2(10f, -10f);

                enemyStatsUi.SetText("");
            }
            else
            {
                SetHideUi(true);
            }
        }

        private void Update()
        {
            if (!CompareTag("Player"))
            {
                Vector3 camNormal = uiCam.transform.forward;
                Vector3 vectorFromCam = transform.position + Vector3.up * height - uiCam.transform.position;
                float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);

                if (camNormDot > 0f)
                    rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(uiCam, transform.position + Vector3.up * height)
                            + new Vector2((-uiCam.pixelWidth / 2f), 0f);
                else
                    rectTransform.anchoredPosition = new Vector2(uiCam.pixelWidth * 2, uiCam.pixelHeight * 2);
            }
        }

        public void ChangeHp(int changeValue)
        {
            int newHp = Mathf.Clamp(hp + changeValue, 0, maxHp);
            if (hp == newHp)
                return;

            hp = newHp;
            enemyStatsUi.SetHp(hp);

            foreach (EntityStatsObserver obs in entityStatsObservers)
                obs.ChangedHp(changeValue);
        }

        public void SetHideUi(bool hide)
        {
            enemyStatsUi.SetHidden(hide);
        }

        public void ChangeMp(int changeValue)
        {
            mp = Mathf.Clamp(mp + changeValue, 0, maxMp);
            enemyStatsUi.SetMp(mp);
        }

        private void OnDestroy()
        {
            Destroy(statsUi);
        }
    }
}
