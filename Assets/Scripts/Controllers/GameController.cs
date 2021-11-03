using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class GameController : MonoBehaviour
    {
        public static float uiScaleFactor = 1f;

        public PlayerController gamePlayer;
        public PlayerController metaPlayer;

        [Space(10)]
        public DialogueManager dialogue;
        public DialogueBubble dialogueBubble;

        [Space(10)]
        public PlayerEventManager playerEventManager;
        public HorrorEventManager horrorEventManager;
        public QuestManager questManager;
        public StoryManager storyManager;
        public MusicManager musicManager;
        public SaveManager saveManager;

        [Space(10)]
        public Transform meta3dAudio;
        [Range(-1f, 1f)] public float gameAudioPan;
        [Range(0f, 1f)] public float gameAudioFxStrength;

        [Space(10)]
        public MetaHouseController metaHouseController;
        public Transform entityStats;
        public OverallStats overallStats;
        public bool inPcMode;
        public Animator gameGuiFxAnimator;
        public Animator fadingAnimator;

        private int mouseSemaphore = 1;

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

        private void Start()
        {
            LockMouse();
            saveManager.LoadGame();
        }

        public void LockMouse()
        {
            if (--mouseSemaphore == 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void UnlockMouse()
        {
            if (++mouseSemaphore == 1)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
