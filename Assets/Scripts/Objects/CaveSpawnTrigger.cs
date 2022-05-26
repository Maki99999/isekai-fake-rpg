using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CaveSpawnTrigger : MonoBehaviour
    {
        public bool on;
        public SpawnerConditional[] spawners;
        public SpawnerConditional[] goblinSpawnersWave1;
        public SpawnerConditional[] goblinSpawnersWave2;
        public SpawnerConditional bossSpawner;

        private int goblinWave;
        private bool firstTime = true;

        public void SetGoblinWave(int count)
        {
            goblinWave = count;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (on && firstTime)
                {
                    GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.JUMP_GAME);
                    GameController.Instance.controlsHelper.ShowControl(ControlsHelper.Control.SNEAK_GAME);
                    firstTime = false;
                }

                foreach (SpawnerConditional spawner in spawners)
                {
                    if (on) spawner.SpawnObjects();
                    else spawner.ResetObjects();
                }

                foreach (SpawnerConditional spawner in goblinSpawnersWave1)
                {
                    if (on && goblinWave >= 1) spawner.SpawnObjects();
                    else spawner.ResetObjects();
                }

                foreach (SpawnerConditional spawner in goblinSpawnersWave2)
                {
                    if (on && goblinWave >= 2) spawner.SpawnObjects();
                    else spawner.ResetObjects();
                }

                if (on && goblinWave >= 3) bossSpawner.SpawnObjects();
                else bossSpawner.ResetObjects();
            }
        }
    }
}
