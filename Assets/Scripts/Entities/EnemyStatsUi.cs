using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class EnemyStatsUi : MonoBehaviour
    {
        public Text text;

        [Space(10)]
        public Slider hpSlider;
        public Text hpSliderText;
        public Animator hpSliderTextChangeAnim;
        public Text hpSliderTextChangeText;

        [Space(10)]
        public Slider mpSlider;
        public Text mpSliderText;

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
    }
}
