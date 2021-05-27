using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OutlineCreator: MonoBehaviour
{
    protected List<Outline> outlines;

    protected virtual void Start()
    {
        outlines = new List<Outline>();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            Outline newOutline = renderer.gameObject.AddComponent<Outline>();
            newOutline.enabled = false;
            outlines.Add(newOutline);
        }
    }
}
