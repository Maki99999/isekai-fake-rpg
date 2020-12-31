using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default {
public class PauseManager : MonoBehaviour
{
    static BooleanWrapper paused = new BooleanWrapper(false);
    public static BooleanWrapper isPaused() { return paused; }

    List<MonoBehaviour> pausingObjects;

    void Start()
    {
        pausingObjects = new List<MonoBehaviour>((MonoBehaviour[])Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour)));
        foreach (MonoBehaviour pausingObject in pausingObjects.ToArray())
        {
            if (!(pausingObject is Pausing))
            {
                pausingObjects.Remove(pausingObject);
            }
        }
    }

    public void Pause()
    {
        paused.Value = true;
        foreach (MonoBehaviour pausingObject in pausingObjects)
        {
            if (pausingObject.isActiveAndEnabled)
                ((Pausing)pausingObject).Pause();
        }
    }

    public void UnPause()
    {
        foreach (MonoBehaviour pausingObject in pausingObjects)
        {
            if (pausingObject.isActiveAndEnabled)
                ((Pausing)pausingObject).UnPause();
        }
        paused.Value = false;
    }
}

public interface Pausing
{
    void Pause();
    void UnPause();
}
}