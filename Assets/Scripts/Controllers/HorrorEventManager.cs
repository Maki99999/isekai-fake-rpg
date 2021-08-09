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
        public H6SpiderCrawl eventH6Crawl;
        public H8Mirror eventH8Mirror;
        public H11Glass eventH11Glass;
        public InvincibleJumpingSpider eventH13JumpingSpider;

        public bool StartEvent(string eventId)
        {
            return eventId switch
            {
                "H1" => EventH1(),
                "H3" => EventH3(),
                "H4" => EventH4(),
                "H5" => EventH5(),
                "H6" => EventH6(),
                "H8" => EventH8(),
                "H11" => EventH11(),
                "H13" => EventH13(),

                //DebugEvents
                "H908" => EventH908(),
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

        private bool EventH6()
        {
            eventH6Crawl.Crawl();
            return true;
        }

        private bool EventH8()
        {
            eventH8Mirror.ShowGhost();
            return true;
        }

        private bool EventH11()
        {
            eventH11Glass.FallingGlass();
            return true;
        }

        private bool EventH13()
        {
            eventH13JumpingSpider.StartJump();
            return true;
        }

        //DebugEvents
        private bool EventH908()
        {
            eventH8Mirror.HideGhost();
            return true;
        }
    }
}
