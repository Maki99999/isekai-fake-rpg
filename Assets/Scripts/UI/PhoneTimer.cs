using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneTimer : MonoBehaviour
{
    public Text text;
    public Text textMs;

    bool showMiliseconds = false;
    float endTime = 0f;

    public void StartTimer(int minutes, float timeMultiplier)
    {
        StopAllCoroutines();
        StartCoroutine(TimerAnim(minutes, timeMultiplier));
    }

    public void SkipTime(float seconds)
    {
        endTime -= seconds;
    }

    IEnumerator TimerAnim(int minutesTotal, float timeMultiplier)
    {
        yield return new WaitForSeconds(1.5f);
        text.text = string.Format("{0:D2}:00", Mathf.FloorToInt(minutesTotal / 10f));
        yield return new WaitForSeconds(0.3f);
        text.text = string.Format("{0:D2}:00", minutesTotal);
        yield return new WaitForSeconds(0.3f);

        showMiliseconds = true;
        endTime = Time.time + (minutesTotal * 60f) * timeMultiplier;
        while (Time.time < endTime)
        {
            float visibleTimeSeconds = (endTime - Time.time) / timeMultiplier;
            int minutes = Mathf.FloorToInt(visibleTimeSeconds / 60f);
            int seconds = Mathf.FloorToInt(visibleTimeSeconds % 60f);
            text.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            yield return new WaitForSeconds(0.5f * timeMultiplier);
        }
        OnDisable();
    }

    void Update()
    {
        if (showMiliseconds)
        {
            textMs.text = string.Format(".{0:D2}", Random.Range(0, 100));
        }
    }

    public void OnDisable()
    {
        showMiliseconds = false;
        text.text = "00:00";
        textMs.text = ".00";
    }
}
