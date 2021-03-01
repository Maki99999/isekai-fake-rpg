using System.Collections;
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

        public void Pause()
        {
            paused.Value = true;
            Time.timeScale = 0;
            //foreach (Pausing pausingObject in pausingObjects)
            //{
            //    pausingObject.Pause();
            //}
        }

        public void UnPause()
        {
            Time.timeScale = 1;
            //foreach (Pausing pausingObject in pausingObjects)
            //{
            //    pausingObject.UnPause();
            //}
            paused.Value = false;
        }
    }

    //public interface Pausing
    //{
    //    void Pause();
    //    void UnPause();
    //}
}