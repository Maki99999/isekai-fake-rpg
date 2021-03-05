using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class EntityStats : MonoBehaviour
    {
        public static float uiScaleFactor = 1f;

        public string displayName;
        public int hp;
        public int maxHp;
        public int mp;
        public int maxMp;
        public float level;
        public bool isInvincible = false;
        public float height;

        [HideInInspector] public bool mpGainLocked = false;

        protected Camera uiCam;
        protected GameObject statsUi;
        protected RectTransform rectTransform;
        protected EntityStatsUi entityStatsUi;
        protected Transform entityStats;

        public GameObject statsUiPrefab;

        [HideInInspector] public List<EntityStatsObserver> entityStatsObservers = new List<EntityStatsObserver>();

        protected bool isHidden = true;

        void Start()
        {
            entityStats = GameController.Instance.entityStats;
            uiCam = GameController.Instance.gamePlayer.cam;

            statsUi = Instantiate(statsUiPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), entityStats);
            entityStatsUi = statsUi.GetComponentInChildren<EntityStatsUi>();
            entityStatsUi.SetMaxHp(maxHp, maxHp);
            entityStatsUi.SetMaxMp(maxMp, maxMp);
            if (isInvincible || maxHp <= 0)
                entityStatsUi.SetHpActive(false);
            if (maxMp <= 0)
                entityStatsUi.SetMpActive(false);

            rectTransform = statsUi.GetComponent<RectTransform>();
            rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.pivot = new Vector2(0.5f, 0f);
            entityStatsUi.SetText(displayName + " Lvl. " + Mathf.FloorToInt(level));

            SetHideUi(true);
        }

        private void Update()
        {
            if (!isHidden)
            {
                Vector3 camNormal = uiCam.transform.forward;
                Vector3 vectorFromCam = transform.position + Vector3.up * height - uiCam.transform.position;
                float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);

                if (camNormDot > 0f)
                    rectTransform.anchoredPosition = uiCam.WorldToScreenPoint(transform.position + Vector3.up * height) * uiScaleFactor;
                else
                    rectTransform.anchoredPosition = new Vector2(uiCam.pixelWidth * -1, uiCam.pixelHeight * -1);
            }
        }

        public void ChangeHp(int changeValue)
        {
            int newHp = Mathf.Clamp(hp + changeValue, 0, maxHp);
            if (hp == newHp)
                return;

            hp = newHp;
            entityStatsUi.SetHp(hp);

            foreach (EntityStatsObserver obs in entityStatsObservers)
                obs.ChangedHp(changeValue);
        }

        public void SetHideUi(bool hide)
        {
            isHidden = hide;
            entityStatsUi.SetHidden(hide);
        }

        public void ChangeMp(int changeValue)
        {
            if (!(mpGainLocked && changeValue > 0f))
            {
                mp = Mathf.Clamp(mp + changeValue, 0, maxMp);
                entityStatsUi.SetMp(mp);
            }
        }

        void OnDestroy()
        {
            Destroy(statsUi);
        }

        public void SetZValue(float zValue)
        {
            entityStatsUi.zValue = zValue;
        }
    }
}
