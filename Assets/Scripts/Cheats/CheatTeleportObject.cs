using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CheatTeleportObject : MonoBehaviour, Useable
    {
        public Transform position;
        public bool inGame = false;

        public void LookingAt() { }

        public void Use()
        {
            GameController.Instance.playerEventManager.TeleportPlayer(inGame, position);
        }
    }
}
