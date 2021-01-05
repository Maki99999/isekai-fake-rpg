using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class GameController : MonoBehaviour
    {
        public PlayerController player;
        public Transform entityStats;

        private static GameController _instance;

        public static GameController Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
