using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class GamePlayerDeath : MonoBehaviour
    {
        public PlayerController playerController;
        public Transform waitPosition;
        public Transform[] respawnPositions;

        public PlayerStats playerStats;

        private void OnEnable()
        {
            StartCoroutine(DeathAnim());
        }

        private IEnumerator DeathAnim()
        {
            playerController.SetFrozen(true);
            float hpRegenMultiplier = playerStats.statsRegen.hpRegenMultiplier;
            playerStats.statsRegen.hpRegenMultiplier = 0f;
            yield return new WaitForSeconds(1.5f);
            Transform respawnPos = GetNearestRespawnPosition();
            playerController.TeleportPlayer(waitPosition);

            yield return new WaitForSeconds(5f);
            playerStats.ChangeHp(playerStats.maxHp);
            playerController.TeleportPlayer(respawnPos);
            playerStats.statsRegen.hpRegenMultiplier = hpRegenMultiplier;
            GameController.Instance.musicManager.ChangeMusic(MusicType.TRAVEL);

            yield return new WaitForSeconds(.5f);
            playerController.SetFrozen(false);
            gameObject.SetActive(false);
        }

        private Transform GetNearestRespawnPosition()
        {
            Vector3 playerPos = playerController.transform.position;
            Transform nearestPos = playerController.transform;
            float nearestDist = float.MaxValue;

            foreach (Transform respawnPosition in respawnPositions)
            {
                float distanceSqr = (playerPos - respawnPosition.position).sqrMagnitude;
                if (distanceSqr < nearestDist)
                {
                    nearestPos = respawnPosition;
                    nearestDist = distanceSqr;
                }
            }

            return nearestPos;
        }
    }
}
