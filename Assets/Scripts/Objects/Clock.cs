using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform handHours;
    public Transform handMinutes;
    public Transform handSeconds;

    public void SetTime(int hour, int minute)
    {
        handHours.localRotation = Quaternion.Euler((-360f / 12f) * hour + (-360f / (12f * 60f)) * minute, 0, 0);
        handMinutes.localRotation = Quaternion.Euler((-360f / 60f) * minute, 0, 0);
    }

    void Start()
    {
        StartCoroutine(Tick());

        SetTime(6, 25);
    }

    IEnumerator Tick()
    {
        while (enabled)
        {
            handSeconds.Rotate((-360f / 60f), 0, 0, Space.Self);
            yield return new WaitForSeconds(1f);
        }
    }
}
