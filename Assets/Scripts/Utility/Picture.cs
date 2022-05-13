using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class Picture : MonoBehaviour
    {
        [SerializeField] new private Renderer renderer;
        [SerializeField] private Texture[] textures;

        void Start()
        {
            Texture texture = textures[Random.Range(0, textures.Length)];
            if ((texture.height / (float)texture.width) > 1f)
                transform.localScale = new Vector3(1f, 1f, (texture.width / (float)texture.height));
            else
                transform.localScale = new Vector3(1f, (texture.height / (float)texture.width), 1f);
            renderer.material.mainTexture = texture;
        }
    }
}
