using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class GameController : MonoBehaviour
    {
        public PlayerController gamePlayer;
        public PlayerController metaPlayer;
        public DialogueManager dialogue;
        public EventManager eventManager;
        public Transform entityStats;
        public Transform meta3dAudio;
        [Range(-1f, 1f)] public float gameAudioPan;
        [Range(0f, 1f)] public float gameAudioFxStrength;
        public bool inPcMode;

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

        public void PlayMeta3dSound(AudioSource audio) {
            //TODO: Make empty; copy audioSource component to it; set parent to; destroy(audioClipLength)
        }
    }
}
