using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ActivateOnDeath : MonoBehaviour, EntityStatsObserver
    {
        public EntityStats entityStats;
        public GameObject[] gameObjects;

        public bool deactivateOnStart = false;

        void Start()
        {
            entityStats.entityStatsObservers.Add(this);

            if (deactivateOnStart)
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.SetActive(false);
            }
        }

        public void ChangedHp(int change)
        {
            if (entityStats.hp <= 0)
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.SetActive(true);
            }
        }
    }
}
