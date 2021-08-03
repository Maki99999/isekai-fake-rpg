using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MetaHouseController : MonoBehaviour
{
    private UsesPower[] usesPowers;

    private void Awake()
    {
        usesPowers = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<UsesPower>().ToArray();
    }

    public void SetPower(bool powerOn)
    {
        foreach (UsesPower usesPower in usesPowers)
            usesPower.SetPower(powerOn);
    }
}
