using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StoryManager : MonoBehaviour
    {
        public bool startWithTrailer;
        public Trailer trailer;

        [Space(10)]
        public GameObject t4Obj;

        private string currentTaskId = "";

        void Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            if (startWithTrailer)
                trailer.StartTrailer();
        }

        public bool isTaskBlockingPc()
        {
            switch (currentTaskId)
            {
                default:
                    return false;
            }
        }

        public void StartTask(string id)
        {
            currentTaskId = id;
            switch (id)
            {
                case "T4":
                    t4Obj.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
