using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerConditional : MonoBehaviour
{
    public GameObject[] originalObjects;
    List<GameObject> instantiatedObjects = new List<GameObject>();

    protected virtual void Start()
    {
        foreach (GameObject orig in originalObjects)
        {
            orig.SetActive(false);
        }
    }

    public virtual void SpawnObjects()
    {
        ResetObjects();
        foreach (GameObject orig in originalObjects)
        {
            GameObject instObj = Instantiate(orig, orig.transform.position, orig.transform.rotation, orig.transform.parent);
            instObj.SetActive(true);
            instantiatedObjects.Add(instObj);
        }
    }

    public virtual void ResetObjects()
    {
        foreach (GameObject orig in instantiatedObjects)
        {
            if (orig != null)
                Destroy(orig);
        }
    }
}
