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

        public void Trigger()
        {
            if (triggered)
                return;
            triggered = true;

            GameController.Instance.horrorEventManager.StartEvent("H12");

            normalWindow.SetActive(false);
            brokenWindow.SetActive(true);
            audio1.PlayDelayed(0.25f);
            audio2.PlayDelayed(0.65f);
        }
    }
}
