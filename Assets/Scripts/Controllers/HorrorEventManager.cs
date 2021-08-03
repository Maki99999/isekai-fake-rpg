using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class HorrorEventManager : MonoBehaviour
    {
        public GameObject eventH5Object;
        public FuseBox eventH4FuseBox;

        public bool StartEvent(string eventId)
        {
            return eventId switch
            {
                "H4" => EventH4(),
                "H5" => EventH5(),
                _ => false
            };
        }

        private bool EventH5()
        {
            eventH5Object.SetActive(true);
            return true;
        }

        private bool EventH4()
        {
            eventH4FuseBox.PowerOff();
            return true;
        }
    }
}
