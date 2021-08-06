using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class HorrorEventManager : MonoBehaviour
    {
        public PcController eventH1PcController;
        public WallClock eventH1Clock;
        public H3Window eventH3Window;
        public H4FuseBox eventH4FuseBox;
        public GameObject eventH5Object;

        public bool StartEvent(string eventId)
        {
            return eventId switch
            {
                "H1" => EventH1(),
                "H3" => EventH3(),
                "H4" => EventH4(),
                "H5" => EventH5(),
                _ => false
            };
        }

        private bool EventH1()
        {
            eventH1PcController.lookAtPhone = true;
            eventH1Clock.enabled = false;
            return true;
        }

        private bool EventH3()
        {
            return eventH3Window.Trigger();
        }

        private bool EventH4()
        {
            eventH4FuseBox.PowerOff();
            return true;
        }

        private bool EventH5()
        {
            eventH5Object.SetActive(true);
            return true;
        }
    }
}
