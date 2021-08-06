using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneTimerText : MonoBehaviour
{
    public Text text;

    bool started = false;

    public void StartText()
    {
        started = true;
    }

    void Update()
    {
        if (started)
        {
            text.text = string.Format(".{0:D2}", Random.Range(0, 100));
        }
    }

    public void StopText()
    {
        started = false;
        text.text = ".00";
    }
}
