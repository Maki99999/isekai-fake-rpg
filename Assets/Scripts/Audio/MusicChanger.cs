using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class MusicChanger : MonoBehaviour
    {
        public MusicType musicType;

        MusicManager musicManager;

        void Start()
        {
            musicManager = GameController.Instance.musicManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                musicManager.ChangeMusic(musicType);
            }
        }
    }
}
