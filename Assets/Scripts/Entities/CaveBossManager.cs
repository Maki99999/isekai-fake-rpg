using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CaveBossManager : MonoBehaviour
    {
        public GameObject boss;
        GameObject instBoss;

        public GameObject blockage;
        public Transform playerStartPos;
        public Transform cutsceneLookPos;
        public AudioSource screamSfx;

        bool bossSpawned = false;

        void Start()
        {
            boss.SetActive(false);
            blockage.SetActive(false);
        }

        void SpawnBoss()
        {
            if (bossSpawned)
                return;
            bossSpawned = true;

            instBoss = Instantiate(boss, boss.transform.position, boss.transform.rotation, boss.transform.parent);
            instBoss.SetActive(true);
            blockage.SetActive(true);

            StartCoroutine(Cutscene(instBoss.GetComponent<GenericEnemy>()));
        }

        IEnumerator Cutscene(GenericEnemy moveScript)
        {
            GameController.Instance.eventManager.FreezePlayer(true, true);
            moveScript.enabled = false;
            StartCoroutine(GameController.Instance.eventManager.MoveRotatePlayer(true, cutsceneLookPos, 0.75f, true));
            //yield return null;
            Animator bossAnim = instBoss.GetComponentInChildren<Animator>();
            bossAnim.SetTrigger("Scream");
            bossAnim.speed = 0;

            yield return new WaitForSeconds(0.5f);
            bossAnim.speed = 1;

            yield return new WaitForSeconds(0.95f);
            screamSfx.Play();

            yield return new WaitForSeconds(1.05f);
            yield return GameController.Instance.eventManager.MoveRotatePlayer(true, playerStartPos, 0.5f, false);

            GameController.Instance.eventManager.FreezePlayer(true, false);
            moveScript.enabled = true;
        }

        void DespawnBoss()
        {
            if (!bossSpawned)
                return;
            bossSpawned = false;
            if (instBoss != null)
                Destroy(instBoss);
            blockage.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SpawnBoss();
            }
        }

        void OnDisable()
        {
            DespawnBoss();
            boss.SetActive(false);
        }
    }
}
