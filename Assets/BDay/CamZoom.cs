using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    public float distance;
    public Transform direction;

    public float smoothValue;
    float smooth = 0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            smooth += smoothValue * Time.deltaTime;
        }
        else
        {
            smooth -= smoothValue * Time.deltaTime;
        }
        smooth = Mathf.Clamp(smooth, 0f, distance);
        transform.localPosition = direction.localPosition * smooth;
    }
}
