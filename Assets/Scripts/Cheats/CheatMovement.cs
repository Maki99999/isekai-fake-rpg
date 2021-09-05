using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CheatMovement : MonoBehaviour
    {
        public PlayerController playerController;

        float speedNormal = 1f;
        float speedNormalSprinting = 1f;

        float timeScaleNormal = 1f;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                speedNormal = playerController.speedNormal;
                speedNormalSprinting = playerController.speedSprinting;
                playerController.speedNormal *= 20;
                playerController.speedSprinting *= 20;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                playerController.speedNormal = speedNormal;
                playerController.speedSprinting = speedNormalSprinting;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                timeScaleNormal = Time.timeScale;
                Time.timeScale *= 10;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Time.timeScale = timeScaleNormal;
            }
        }
    }
}
