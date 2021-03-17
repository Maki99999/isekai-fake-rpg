using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Default
{
    public class Spawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs;

        [Space(10)]
        public float range;
        public float playerSafeRange;
        public float minSpawnTime;
        public float maxSpawnTime;
        public int maxEntityCount;

        private List<GameObject> entities = new List<GameObject>();

        private Transform player;

        private void Start()
        {
            player = GameController.Instance.gamePlayer.transform;
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (enabled)
            {
                entities.RemoveAll((e) => e == null);

                for (int i = 0; i < Mathf.CeilToInt((maxEntityCount - entities.Count) / 2f); i++)
                {
                    int tryFindingPosition = 3;
                    Vector3 spawnPosition = transform.position;
                    float distance = 0f;
                    do
                    {
                        Vector3 randDirection = Random.insideUnitSphere * range;
                        randDirection += transform.position;

                        NavMeshHit navHit;
                        NavMesh.SamplePosition(randDirection, out navHit, range, -1);

                        spawnPosition = navHit.position;

                        distance = Vector3.Distance(player.position, spawnPosition);

                    } while (distance < playerSafeRange && --tryFindingPosition > 0);

                    if (tryFindingPosition > 0 && !float.IsInfinity(spawnPosition.x))
                        entities.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPosition, Random.rotation, transform));
                }

                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            }
        }
    }
}
