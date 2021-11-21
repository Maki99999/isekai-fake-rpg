using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ZoomController : MonoBehaviour
    {
        new public Camera camera;
        public Vector2 fovMinMax;

        private float currentFov = 60f;

        private void Start()
        {
            currentFov = fovMinMax.x;
        }

        void Update()
        {
            float desiredFov = InputSettings.PressingZoom() ? fovMinMax.y : fovMinMax.x;

            currentFov = Mathf.Lerp(currentFov, desiredFov, 0.5f);
            if (currentFov < fovMinMax.y + 0.5f)
                currentFov = fovMinMax.y;
            else if (currentFov > fovMinMax.x - 0.5f)
                currentFov = fovMinMax.x;

            camera.fieldOfView = currentFov;
        }
    }
}
