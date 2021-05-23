using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtual3dAudioMetaListenerRoom : MonoBehaviour
{
    public Transform gamePlayer;
    public Transform metaPlayer;

    void Start()
    {
        transform.SetParent(gamePlayer);
    }

    void Update()
    {
        transform.localPosition = -metaPlayer.position;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.RotateAround(gamePlayer.position, Vector3.up, -metaPlayer.eulerAngles.y);
    }
}
