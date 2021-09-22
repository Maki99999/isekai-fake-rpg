using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class HorrorEventManager : MonoBehaviour
    {
        public PhoneHolding eventH1Call;
        public H3Window eventH3Window;
        public H4FuseBox eventH4FuseBox;
        public GameObject eventH5Object;
        public H6SpiderCrawl eventH6Crawl;
        public H8Mirror eventH8Mirror;
        public H10FoodFlesh eventH10FoodFlesh;
        public H11Glass eventH11Glass;
        public InvincibleJumpingSpider eventH13JumpingSpider;
        public Microwave eventH14Microwave;
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
                //"H9" => EventH9(),
                "H10" => EventH10(),
                "H11" => EventH11(),
                "H12" => EventH12(),
                "H13" => EventH13(),
                "H14" => EventH14(),
                "H15" => EventH15(),
                "H16" => EventH16(),
                //H17-H20: completely random

                //DebugEvents
                "H908" => EventH908(),
                "H919" => EventH919(),
                _ => false
            };
        }

        private bool EventH1()
        {
            eventH1Call.gameObject.SetActive(true);
            eventH1Call.H1Call();
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

        private bool EventH10()
        {
            eventH10FoodFlesh.ToggleFlesh();
            return true;
        }

        private bool EventH11()
        {
            eventH11Glass.FallingGlass();
            return true;
        }

        private bool EventH12()
        {
            GameController.Instance.musicManager.ChangeMusic(MusicType.HORROR);
            return true;
        }

        private bool EventH13()
        {
            eventH13JumpingSpider.StartJump();
            return true;
        }

        private bool EventH14()
        {
            eventH14Microwave.Close();
            eventH14Microwave.TurnOn();
            return true;
        }

        private bool EventH15()
        {
            eventH14Microwave.H15Event();
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
