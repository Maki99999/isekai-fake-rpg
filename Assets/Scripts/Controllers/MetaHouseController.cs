using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default
{
    public class MetaHouseController : MonoBehaviour
    {
        private PcController pcController;
        private UsesPower[] usesPowers;
        private Clock[] clocks;

        private bool timeIsAdvancing = false;
        private float timeMultiplier = 1f;
        private float startTime = 0;
        private int fixedHour = 0;
        private int fixedMinute = 0;

        public bool brokenWallClocksAcknowledged = false;

        private void Awake()
        {
            pcController = GameObject.FindObjectOfType<PcController>();
            usesPowers = GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<UsesPower>().ToArray();
            clocks = GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<Clock>().ToArray();
        }

        private void Start()
        {
            SetFixedTime(18, 25);
        }

        public void SetPower(bool powerOn)
        {
            foreach (UsesPower usesPower in usesPowers)
                usesPower.SetPower(powerOn);
        }

        public void SetFixedTime(int hour, int minute)
        {
            fixedHour = hour;
            fixedMinute = minute;
            SetTime(hour, minute);
        }

        public void SetTime(int hour, int minute)
        {
            foreach (Clock clock in clocks)
                clock.SetTime(hour, minute);
        }

        public void LetTimeAdvance(bool advance = true, float timeMultiplier = 1f)
        {
            if (advance)
            {
                startTime = Time.time;
                this.timeMultiplier = timeMultiplier;
                timeIsAdvancing = true;
                StartCoroutine(TimeAdvance());
            }
            else
            {
                timeIsAdvancing = false;
            }
        }

        private IEnumerator TimeAdvance()
        {
            while (timeIsAdvancing)
            {
                float secondsSinceStart = Time.time - startTime;
                float secondsSince0 = secondsSinceStart / timeMultiplier + (60f * 60f * fixedHour) + (60f * fixedMinute);
                int hour = Mathf.FloorToInt(secondsSince0 / (60f * 60f)) % 24;
                int minute = Mathf.FloorToInt(secondsSince0 / 60f) % 60;
                SetTime(hour, minute);

                yield return new WaitForSeconds(0.3f);
            }
        }

        public void StopWallClocks()
        {
            foreach (Clock clock in clocks)
                if (clock is WallClock)
                    ((WallClock)clock).enabled = false;
        }
    }

    public interface Clock
    {
        void SetTime(int hour, int minute);
    }
}
