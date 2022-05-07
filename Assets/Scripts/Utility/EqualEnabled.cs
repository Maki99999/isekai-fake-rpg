using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class EqualEnabled : MonoBehaviour
    {
        public GameObject[] components;

        private void OnEnable()
        {
            foreach (GameObject component in components)
                if (component != null)
                    component.SetActive(true);
        }

        private void OnDisable()
        {
            foreach (GameObject component in components)
                if (component != null)
                    component.SetActive(false);
        }
    }
}
