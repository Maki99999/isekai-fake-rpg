using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OutlineCreator : MonoBehaviour
{
    public GameObject outlinesGameObjectParent = null;
    protected List<Outline> outlines;

    protected virtual void Start()
    {
        if (outlinesGameObjectParent == null)
            outlinesGameObjectParent = gameObject;

        outlines = new List<Outline>();
        MeshRenderer[] renderers = outlinesGameObjectParent.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            Outline newOutline = renderer.gameObject.AddComponent<Outline>();
            newOutline.enabled = false;
            outlines.Add(newOutline);
        }
    }
}
