using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace Default
{
    public class PlayerStatsUi : EntityStatsUi
    {
        [Space(10)]
        public Slider xpSlider;
        public Text coinText;

        public EntityStatsUiGroup entityStatsUiGroup;

        void Start()
        {
            group = entityStatsUiGroup;
            rect = mpSlider.GetComponent<RectTransform>();
            group.stats.Add(this);
        }

        public void ShakeMp()
        {
            StopAllCoroutines();
            StartCoroutine(ShakeMpAnim());
        }

        IEnumerator ShakeMpAnim()
        {
            for (int i = 0; i < 3; i++)
            {
                rect.sizeDelta = new Vector2(230, 20);
                yield return new WaitForSeconds(.2f);
                rect.sizeDelta = new Vector2(250, 20);
                yield return new WaitForSeconds(.2f);
            }
        }

        public void SetSublevel(float sublevel)
        {
            xpSlider.value = sublevel;
        }

        public void SetCoins(int coins) {
            coinText.text = coins.ToString();
        }

        void OnDestroy()
        {
            group.stats.Remove(this);
        }
    }
}
