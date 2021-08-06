using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Default
{
    public class MetaHouseController : MonoBehaviour
    {
        private UsesPower[] usesPowers;
        private Clock[] clocks;

        private void Awake()
        {
            usesPowers = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<UsesPower>().ToArray();
            clocks = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<Clock>().ToArray();
        }

        public void SetPower(bool powerOn)
        {
            foreach (UsesPower usesPower in usesPowers)
                usesPower.SetPower(powerOn);
        }

        public void SetTime(int hour, int minute)
        {
            foreach (Clock clock in clocks)
                clock.SetTime(hour, minute);
        }
    }

    public interface Clock
    {
        void SetTime(int hour, int minute);
    }
}
