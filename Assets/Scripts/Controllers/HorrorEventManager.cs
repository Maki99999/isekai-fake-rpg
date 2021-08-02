using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEventManager : MonoBehaviour
{
    public GameObject eventH5Object;

    public bool StartEvent(string eventId)
    {
        return eventId switch
        {
            "H5" => EventH5(),
            _ => false
        };
    }

    private bool EventH5()
    {
        eventH5Object.SetActive(true);
        return true;
    }
}
