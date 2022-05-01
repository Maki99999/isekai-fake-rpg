using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class OverallStats : MonoBehaviour, ISaveDataObject
    {
        private Dictionary<string, int> stats = new Dictionary<string, int>() { };

        private Dictionary<string, List<OverallStatsObserver>> statsObservers = new Dictionary<string, List<OverallStatsObserver>>();

        public string saveDataId => "OverallStats";

        public void AddToStat(int value, string category)
        {
            if (stats.ContainsKey(category))
                SetStat(stats[category] + value, category);
            else
                SetStat(value, category);
        }

        public void SetStat(int value, string category)
        {
            if (!stats.ContainsKey(category))
                stats.Add(category, 0);

            stats[category] = value;

            if (statsObservers.ContainsKey(category))
                foreach (OverallStatsObserver observer in statsObservers[category])
                    observer.OnStatsUpdated(category);
        }

        public int GetStat(string category)
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
            if (statsObservers.ContainsKey(category))
                statsObservers[category].Remove(observer);
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("StatIdList", new List<string>(stats.Keys));
            foreach (var item in stats)
                entry.Add(item.Key, item.Value);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;

            foreach (var statId in dictEntry.GetList("StatIdList", new List<string>()))
                stats.Add(statId, dictEntry.GetInt(statId, 0));
        }
    }

    public interface OverallStatsObserver
    {
        void OnStatsUpdated(string category);
    }
}
