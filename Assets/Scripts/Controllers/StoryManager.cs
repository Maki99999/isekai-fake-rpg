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
        public T4WashingMachine t4Obj;

        private string currentTaskId = "";

        void Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            if (startWithTrailer)
                trailer.StartTrailer();

            StartTask("T4"); // Debug
        }

        public bool isTaskBlockingPc()
        {
            switch (currentTaskId)
            {
                case "T4":
                    return t4Obj.BlockingPcMode();
                default:
                    return false;
            }
        }

        public void StartTask(string id, bool sideTask = false)
        {
            if (!sideTask)
                currentTaskId = id;
            switch (id)
            {
                case "T4":
                    t4Obj.gameObject.SetActive(true);
                    break;
                case "T10":
                    break;
                default:
                    break;
            }
        }

        public void TaskFinished()
        {
            StartTask("");
        }
    }
}
