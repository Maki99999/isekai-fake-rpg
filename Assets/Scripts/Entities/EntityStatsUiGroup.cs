using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class EntityStatsUiGroup : MonoBehaviour
    {
        public List<EntityStatsUi> stats = new List<EntityStatsUi>();

        void Update()
        {
            List<EntityStatsUi> statsActive = new List<EntityStatsUi>(stats);
            statsActive.RemoveAll((s) => s.isHidden);

            List<float> sortedDistances = new List<float>();
            foreach (EntityStatsUi stat in statsActive)
            {
                stat.SetTransparency(1f);
                sortedDistances.Add(stat.zValue);
            }
            sortedDistances.Sort();

            foreach (EntityStatsUi stat1 in statsActive)
            {
                stat1.transform.SetSiblingIndex(sortedDistances.Count - sortedDistances.FindIndex((f) => f == stat1.zValue));
                foreach (EntityStatsUi stat2 in statsActive)
                {
                    if (stat1 != stat2)
                    {
                        Vector2 a1 = stat1.rectTransform.anchoredPosition;  //ax1, ay2
                        Vector2 a2 = a1 + stat1.rectTransform.sizeDelta;    //ax2, ay1
                        Vector2 b1 = stat2.rectTransform.anchoredPosition;  //bx1, by2
                        Vector2 b2 = b1 + stat2.rectTransform.sizeDelta;    //bx2, by1

                        if (a1.x < b2.x && a2.x > b1.x && a2.y > b1.y && a1.y < b2.y)
                        {
                            Debug.Log("yes");
                            if (stat1.zValue >= stat2.zValue)
                                stat1.SetTransparency(0.2f);
                            else
                                stat2.SetTransparency(0.2f);
                        }
                    }
                }
            }
        }
    }
}
