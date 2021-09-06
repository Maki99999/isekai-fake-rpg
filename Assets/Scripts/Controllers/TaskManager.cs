using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class TaskManager : MonoBehaviour
    {
        public GameObject t1Obj;

        void Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        public void StartTask(string id)
        {
            switch (id)
            {
                case "T1":
                    t1Obj.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
