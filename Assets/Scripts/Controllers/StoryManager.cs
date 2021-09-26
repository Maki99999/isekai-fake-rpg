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
        public T2Oven t2Obj;
        public T4WashingMachine t4Obj;
        public T8Cracker t8Obj;
        public T10Trash t10Obj;
        public T11Dishes t11Obj;
        public T12GetKnife t12Obj;
        public T13WashHands t13Obj;

        public string currentTaskId { get; private set; } = "";

        void Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            if (startWithTrailer)
                trailer.StartTrailer();

            //StartTask("T12"); // Debug
        }

        public bool isTaskBlockingPc()
        {
            switch (currentTaskId)
            {
                case "T2":
                    return t2Obj.BlockingPcMode();
                case "T4":
                    return t4Obj.BlockingPcMode();
                case "T8":
                    return t8Obj.BlockingPcMode();
                case "T12":
                    return t12Obj.BlockingPcMode();
                case "T13":
                    return t13Obj.BlockingPcMode();
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
                case "T2":
                    t2Obj.gameObject.SetActive(true);
                    break;
                case "T4":
                    t4Obj.gameObject.SetActive(true);
                    break;
                case "T8":
                    t8Obj.gameObject.SetActive(true);
                    break;
                case "T10":
                    t10Obj.gameObject.SetActive(true);
                    break;
                case "T11":
                    t11Obj.gameObject.SetActive(true);
                    break;
                case "T12":
                    t12Obj.gameObject.SetActive(true);
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
