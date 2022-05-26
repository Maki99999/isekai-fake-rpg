using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class DistanceTracker : MonoBehaviour
    {
        [SerializeField] private string statNameNormal = "DistanceMovedSqr";
        [SerializeField] private string statNameSneaking = "DistanceMovedSneakingSqr";
        [SerializeField] private string statNameSprinting = "DistanceMovedSprintingSqr";
        [SerializeField] private string statNameJump = "Jumped";
        [SerializeField] private float updateInterval = 0.2f;

        private PlayerController playerController;
        private Vector3 lastPos;
        private float statExact = 0;

        IEnumerator Start()
        {
            playerController = GetComponent<PlayerController>();
            lastPos = transform.position;
            yield return null;

            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(updateInterval);

                float newStatExact = statExact + (lastPos - transform.position).sqrMagnitude;
                int newStatExactInt = Mathf.FloorToInt(newStatExact);
                if (newStatExactInt > Mathf.FloorToInt(statExact))
                {
                    if (statNameNormal != "")
                        GameController.Instance.overallStats.AddToStat(1, statNameNormal);
                    if (playerController.isSneaking && statNameSneaking != "")
                        GameController.Instance.overallStats.AddToStat(1, statNameSneaking);
                    if (playerController.isSprinting && statNameSprinting != "")
                        GameController.Instance.overallStats.AddToStat(1, statNameSprinting);
                    if (statNameJump != "" && lastPos.y + 0.1f < transform.position.y)
                        GameController.Instance.overallStats.AddToStat(1, statNameJump);
                }
                statExact = newStatExact;

                lastPos = transform.position;
            }
        }
    }
}
