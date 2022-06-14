using UnityEngine;
using UnityEngine.Rendering;

namespace Default
{
    [RequireComponent(typeof(Camera))]
    public class NoFogCamera : MonoBehaviour
    {
        private Camera cameraWithoutFog;

        private void Start()
        {
            cameraWithoutFog = GetComponent<Camera>();
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        }
        void OnDestroy()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        }
        void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera == cameraWithoutFog)
                RenderSettings.fog = false;
            else
                RenderSettings.fog = true;
        }
    }
}
