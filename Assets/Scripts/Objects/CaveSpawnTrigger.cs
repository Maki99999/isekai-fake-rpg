using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpawnTrigger : MonoBehaviour
{
    public bool on;
    public SpawnerConditional[] spawners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (SpawnerConditional spawner in spawners)
            {
                if (on)
                    spawner.SpawnObjects();
                else
                    spawner.ResetObjects();
            }
        }
    }
}
