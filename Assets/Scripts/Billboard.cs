using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool onlyYRot = true;
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        if (onlyYRot)
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }
}