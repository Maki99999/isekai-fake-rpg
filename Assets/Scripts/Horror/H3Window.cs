using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H3Window : MonoBehaviour
    {
        public GameObject normalWindow;
        public GameObject brokenWindow;

        public AudioSource audio1;
        public AudioSource audio2;

        bool triggered = false;

        public bool Trigger()
        {
            if (triggered)
                return false;
            triggered = true;

            normalWindow.SetActive(false);
            brokenWindow.SetActive(true);
            audio1.PlayDelayed(0.25f);
            audio2.PlayDelayed(0.65f);

            return true;
        }
    }
}
