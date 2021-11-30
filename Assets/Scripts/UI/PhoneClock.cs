using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class PhoneClock : MonoBehaviour, Clock
    {
        public Text clockText;

        public void SetTime(int hour, int minute)
        {
            if (enabled)
                clockText.text = hour + ":" + minute + "\n";
        }
    }
}
