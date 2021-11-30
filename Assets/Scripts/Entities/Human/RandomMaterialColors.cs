using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialColors : MonoBehaviour
{
    [System.Serializable]
    public struct MeshRendererColor
    {
        public string note;
        public MeshRendererWithNumber[] meshNumbersRenderers;
        public Gradient randomColors;
    }

    [System.Serializable]
    public struct MeshRendererWithNumber
    {
        public SkinnedMeshRenderer meshRenderer;
        public int materialNumber;
    }

    public MeshRendererColor[] meshWithMaterial;

    void Start()
    {
        System.Random randomNumber = new System.Random(GetInstanceID());
        foreach (MeshRendererColor mrc in meshWithMaterial)
        {
            float randValue = (float)randomNumber.NextDouble();
            foreach (MeshRendererWithNumber meshNumbersRenderer in mrc.meshNumbersRenderers)
                meshNumbersRenderer.meshRenderer.materials[meshNumbersRenderer.materialNumber].color = mrc.randomColors.Evaluate(randValue);
        }
    }
}