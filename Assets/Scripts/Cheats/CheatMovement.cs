using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CheatMovement : MonoBehaviour
    {
        public PlayerController playerController;

        bool fastMode = false;
        float speedNormal = 1f;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                fastMode = !fastMode;
                if (fastMode)
                {
                    speedNormal = playerController.speedNormal;
                    playerController.speedNormal *= 20;
                }
                else
                {
                    playerController.speedNormal = speedNormal;
                }
            }
        }
    }
}
