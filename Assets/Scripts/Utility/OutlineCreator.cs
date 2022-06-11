using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public abstract class OutlineCreator : MonoBehaviour
    {
        public GameObject outlinesGameObjectParent = null;
        private List<Outline> outlines;

        protected OutlineHelper outlineHelper;

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

            outlineHelper = new OutlineHelper(this, outlines.ToArray());
        }
    }
}