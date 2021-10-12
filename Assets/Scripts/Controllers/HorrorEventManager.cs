using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class HorrorEventManager : MonoBehaviour
    {
        public PhoneHolding eventH1Call;
        public GameObject eventH3Window;
        public H4FuseBox eventH4FuseBox;
        public GameObject eventH5Object;
        public GameObject eventH6Crawl;
        public H8Mirror eventH8Mirror;
        public H9MonsterBasement eventH9MonsterBasement;
        public GameObject eventH11Glass;
        public GameObject eventH13JumpingSpider;
        public GameObject eventH14Microwave;
        public H16Puppet eventH16Puppet;

        public bool StartEvent(string eventId)
        {
            return eventId switch
            {
                "H1" => EventH1(),
                //H2: moved to tasks
                "H3" => EventH3(),
                "H4" => EventH4(),
                "H5" => EventH5(),
                "H6" => EventH6(),
                "H7" => EventH7(),
                "H8" => EventH8(),
                "H9" => EventH9(),
                //H10: activated automatically
                "H11" => EventH11(),
                "H12" => EventH12(),
                "H13" => EventH13(),
                "H14" => EventH14(),
                //H15: with H14
                "H16" => EventH16(),
                //H17-H20: completely random

                //DebugEvents
                "H908" => EventH908(),
                "H919" => EventH919(),
                _ => false
            };
        }

        public void StartEventsAfterT8()
        {
            StartEvent("H12");
            StartEvent("H3");
        }

        private bool EventH1()
        {
            eventH1Call.H1Call();
            return true;
        }

        private bool EventH3()
        {
            eventH3Window.SetActive(true);
            return true;
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
            eventH6Crawl.SetActive(true);
            return true;
        }

        private bool EventH7()
        {
            H7SpiderHide[] spiders = GameObject.FindObjectsOfType<H7SpiderHide>();

            foreach (H7SpiderHide spider in spiders)
                spider.Show();
            return true;
        }

        private bool EventH8()
        {
            eventH8Mirror.ShowGhost();
            return true;
        }

        private bool EventH9()
        {
            eventH9MonsterBasement.gameObject.SetActive(true);
            return true;
        }

        private bool EventH11()
        {
            eventH11Glass.SetActive(true);
            return true;
        }

        private bool EventH12()
        {
            GameController.Instance.musicManager.ChangeMusic(MusicType.HORROR);
            return true;
        }

        private bool EventH13()
        {
            eventH13JumpingSpider.SetActive(true);
            return true;
        }

        private bool EventH14()
        {
            eventH14Microwave.SetActive(true);
            return true;
        }

        private bool EventH16()
        {
            eventH16Puppet.enabled = true;
            return true;
        }

        //DebugEvents
        private bool EventH908()
        {
            eventH8Mirror.HideGhost();
            return true;
        }

        private bool EventH919()
        {
            Lamp.randomChance = 1f;
            return true;
        }
    }
}
