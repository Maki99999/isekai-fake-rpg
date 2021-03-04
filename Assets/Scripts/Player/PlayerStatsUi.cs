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

        void Start()
        {
            group = GetComponentInParent<EntityStatsUiGroup>();
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
                rect.sizeDelta = new Vector2(180, 20);
                yield return new WaitForSeconds(.2f);
                rect.sizeDelta = new Vector2(200, 20);
                yield return new WaitForSeconds(.2f);
            }
        }

        public void SetSublevel(float sublevel)
        {
            xpSlider.value = sublevel;
        }

        void OnDestroy()
        {
            group.stats.Remove(this);
        }
    }
}
