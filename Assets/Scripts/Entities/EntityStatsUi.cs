using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace Default
{
    public class EntityStatsUi : MonoBehaviour
    {
        public CanvasGroup cGroup;
        public RectTransform rectTransform;
        public Text text;

        [Space(10)]
        public Slider hpSlider;
        public Text hpSliderText;
        public Animator hpSliderTextChangeAnim;
        public Text hpSliderTextChangeText;

        [Space(10)]
        public Slider mpSlider;
        public Text mpSliderText;

        [HideInInspector] public float zValue;
        [HideInInspector] public bool isHidden = true;

        private EntityStatsUiGroup group;

        private RectTransform rect;

        void Start()
        {
            group = GetComponentInParent<EntityStatsUiGroup>();
            rect = mpSlider.GetComponent<RectTransform>();
            group.stats.Add(this);
        }

        public void SetTransparency(float transparency)
        {
            if (!isHidden)
                cGroup.alpha = transparency;
        }

        public void SetText(string value)
        {
            text.text = value;
        }

        public void SetHp(int value)
        {
            if (hpSlider.gameObject.activeSelf && "" + value != hpSliderText.text)
            {
                hpSliderTextChangeAnim.SetTrigger("Play");
                hpSliderTextChangeText.text = "" + (value - int.Parse(hpSliderText.text));
                hpSlider.value = value;
                hpSliderText.text = "" + value;
            }
        }

        public void SetMaxHp(int value)
        {
            if (hpSlider.gameObject.activeSelf)
            {
                hpSlider.maxValue = value;
                hpSlider.value = value;
                hpSliderText.text = "" + value;
            }
        }

        public void SetMp(int value)
        {
            if (mpSlider.gameObject.activeSelf)
            {
                mpSlider.value = value;
                mpSliderText.text = "" + value;
            }
        }

        public void ShakeMp()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeMpAnim());
        }

        private IEnumerator ShakeMpAnim()
        {
            for (int i = 0; i < 3; i++)
            {
                rect.sizeDelta = new Vector2(180, 20);
                yield return new WaitForSeconds(.2f);
                rect.sizeDelta = new Vector2(200, 20);
                yield return new WaitForSeconds(.2f);
            }
        }

        public void SetMaxMp(int value)
        {
            if (mpSlider.gameObject.activeSelf)
            {
                mpSlider.maxValue = value;
                mpSlider.value = value;
                mpSliderText.text = "" + value;
            }
        }

        public void SetHpActive(bool value)
        {
            hpSlider.gameObject.SetActive(value);
        }

        public void SetMpActive(bool value)
        {
            mpSlider.gameObject.SetActive(value);
        }

        public void SetHidden(bool hidden)
        {
            isHidden = hidden;
            if (hidden)
                cGroup.alpha = 0f;
            else
                cGroup.alpha = 1f;
        }

        private void OnDestroy()
        {
            group.stats.Remove(this);
        }
    }
}
