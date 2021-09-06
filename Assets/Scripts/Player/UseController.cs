using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class UseController : MonoBehaviour
    {
        public PlayerController playerController;
        public float range = 2.5f;
        public LayerMask mask;
        Transform cam;

        bool lastPress = false;

        void LateUpdate()
        {
            if (cam == null)
                cam = playerController.eyeHeightTransform;

            //Get Input
            bool useKey = InputSettings.PressingUse();

            //!(IsRiding & pressing)
            if (!playerController.IsFrozen())  //if (!(useKey && playerController.currentRide != null))
            {
                //Get useable GameObject and maybe use it
                RaycastHit hit;
                if (Physics.Raycast(cam.position, cam.forward, out hit, range, mask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("Useable"))
                    {
                        Useable useable = hitObject.GetComponent<Useable>();

                        if (useable == null)
                        {
                            Debug.LogError("Can't find 'Useable' script.");
                        }
                        else
                        {
                            useable.LookingAt();

                            if (useKey && !lastPress)
                                useable.Use();
                        }
                    }
                }
            }
            lastPress = useKey;
        }
    }

    public interface Useable
    {
        public abstract void LookingAt();
        public abstract void Use();
    }
}