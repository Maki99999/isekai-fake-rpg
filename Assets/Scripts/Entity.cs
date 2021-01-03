using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public abstract class Entity : MonoBehaviour
    {
        public string displayName;
        public int hp;
        public int maxHp;
        public int mp;
        public int level;
        public bool isInvincible = false;
        public float height;

        private Canvas canvas;
        private Camera uiCam;
        private GameObject statsUi;
        private RectTransform rectTransform;
        private Text text;

        public GameObject statsUiPrefab;
        public Transform EntityStats;

        protected void OnStart()
        {
            canvas = EntityStats.GetComponentInParent<Canvas>();
            uiCam = canvas.worldCamera;

            statsUi = Instantiate(statsUiPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), EntityStats);
            text = statsUi.GetComponentInChildren<Text>();

            rectTransform = statsUi.GetComponent<RectTransform>();
            rectTransform.localRotation = Quaternion.Euler(Vector3.zero);
            rectTransform.localPosition = Vector3.zero;

            if (CompareTag("Player"))
            {
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchoredPosition = new Vector2(10f, -10f);
            }
        }

        protected void OnUpdate()
        {
            if (!CompareTag("Player"))
            {
                Vector3 camNormal = uiCam.transform.forward;
                Vector3 vectorFromCam = transform.position + Vector3.up * height - uiCam.transform.position;
                float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
                if (camNormDot > 0f)
                {
                    text.text = displayName + "\nHP: " + hp + ", MP: " + mp;

                    rectTransform.anchoredPosition = RectTransformUtility.WorldToScreenPoint(uiCam, transform.position + Vector3.up * height)
                            + new Vector2((-uiCam.pixelWidth / 2f), 0f);
                }
                else
                {
                    rectTransform.anchoredPosition = new Vector2(uiCam.pixelWidth * 2, uiCam.pixelHeight * 2);
                }
            }
            else
            {
                text.text = "HP: " + hp + ", MP: " + mp;
            }
        }

        public void ChangeHealth(int changeValue)
        {
            hp = Mathf.Clamp(hp + changeValue, 0, maxHp);

            if (hp <= 0)
                Die();
        }

        public virtual void Die()
        {
            Destroy(statsUi);
            Destroy(gameObject);
        }
    }
}
