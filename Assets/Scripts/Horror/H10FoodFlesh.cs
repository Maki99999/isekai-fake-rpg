using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H10FoodFlesh : MonoBehaviour
    {
        public GameObject flesh;
        public GameObject food;

        public AudioSource sound;

        bool fleshOn = false;

        public void ShowFlesh(bool show)
        {
            fleshOn = show;
            flesh.SetActive(show);
            food.SetActive(!show);
            sound.Play();
        }

        public void ToggleFlesh()
        {
            ShowFlesh(!fleshOn);
        }
    }
}
