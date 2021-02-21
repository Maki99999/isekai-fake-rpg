using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialColors : MonoBehaviour
{
    [System.Serializable]
    public struct MeshRendererColor
    {
        public SkinnedMeshRenderer meshRenderer;
        public int materialNumber;
        public Gradient randomColors; 
    }

    public MeshRendererColor[] meshWithMaterial;

    void Start()
    {
        foreach (MeshRendererColor mrc in meshWithMaterial)
        {
            mrc.meshRenderer.materials[mrc.materialNumber].color = mrc.randomColors.Evaluate(Random.value);
        }
    }
}