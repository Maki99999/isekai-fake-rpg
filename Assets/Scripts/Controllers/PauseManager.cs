﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class PauseManager : MonoBehaviour
    {
        static BooleanWrapper paused = new BooleanWrapper(false);
        public static BooleanWrapper isPaused() { return paused; }

        //List<Pausing> pausingObjects;

        void Start()
        {
            //pausingObjects = new List<Pausing>();
        }

        public static void Pause()
        {
            paused.Value = true;
            Time.timeScale = 0;
        }

        public static void Unpause()
        {
            Time.timeScale = 1;
            paused.Value = false;
        }
    }
}