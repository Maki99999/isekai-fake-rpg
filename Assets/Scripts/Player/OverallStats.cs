using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class OverallStats : MonoBehaviour
    {
        private Dictionary<string, float> stats = new Dictionary<string, float>() { };

        private Dictionary<string, List<OverallStatsObserver>> statsObservers = new Dictionary<string, List<OverallStatsObserver>>();

        public void AddToStat(int value, string category)
        {
            if (!stats.ContainsKey(category))
                stats.Add(category, 0);

            stats[category] += value;

            string[] keys = new string[statsObservers.Keys.Count];
            statsObservers.Keys.CopyTo(keys, 0);

            if (statsObservers.ContainsKey(category))
                foreach (OverallStatsObserver observer in statsObservers[category])
                    observer.OnStatsUpdated();
        }

        public float GetStat(string category)
        {
            if (!stats.ContainsKey(category))
                return 0;
            return stats[category];
        }

        public void AddObserver(OverallStatsObserver observer, string category)
        {
            if (!statsObservers.ContainsKey(category))
                statsObservers.Add(category, new List<OverallStatsObserver>());

            statsObservers[category].Add(observer);
        }

        public void RemoveObserver(OverallStatsObserver observer, string category)
        {
            if (!statsObservers.ContainsKey(category))
                statsObservers[category].Remove(observer);
        }
    }

    public interface OverallStatsObserver
    {
        void OnStatsUpdated();
    }
}
