using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CheatSpeed : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale * 10f, 0.1f, 1000f);
                Debug.Log("TimeScale = " + Time.timeScale);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale / 10f, 0.1f, 1000f);
                Debug.Log("TimeScale = " + Time.timeScale);
            }
        }
    }
}
