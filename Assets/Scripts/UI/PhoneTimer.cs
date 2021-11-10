using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneTimer : MonoBehaviour
{
    public Text text;
    public Text textMs;

    bool showMiliseconds = false;
    bool start = false;
    float endTime = 0f;
    float timeMultiplier = 1f;

    public void PrepareTimer(float endTime, float timeMultiplier)
    {
        StopAllCoroutines();
        this.endTime = endTime;
        this.timeMultiplier = timeMultiplier;
        StartCoroutine(TimerAnim());
    }

    public void StartTime()
    {
        start = true;
    }

    public void SetTime(float endTime)
    {
        this.endTime = endTime;
    }

    IEnumerator TimerAnim()
    {
        start = false;
        float visibleTimeLeft = (endTime - Time.time) * timeMultiplier;
        yield return new WaitForSeconds(1.5f);
        text.text = string.Format("{0:D2}:00", Mathf.FloorToInt(visibleTimeLeft / 600f));
        yield return new WaitForSeconds(0.3f);
        text.text = string.Format("{0:D2}:00", Mathf.FloorToInt(visibleTimeLeft / 60f));

        yield return new WaitUntil(() => start);

        showMiliseconds = true;
        while (Time.time < endTime)
        {
            visibleTimeLeft = (endTime - Time.time) * timeMultiplier;
            int minutes = Mathf.FloorToInt(visibleTimeLeft / 60f);
            int seconds = Mathf.FloorToInt(visibleTimeLeft % 60f);
            text.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            yield return new WaitForSeconds(0.5f);
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
        StopAllCoroutines();
        showMiliseconds = false;
        text.text = "00:00";
        textMs.text = ".00";
    }
}
