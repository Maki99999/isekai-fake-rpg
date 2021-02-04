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

        void LateUpdate()
        {
            if (cam == null)
                cam = playerController.camTransform;


            //Get Input
            bool useKey = Input.GetKeyDown(GlobalSettings.keyUse) || Input.GetKeyDown(GlobalSettings.keyUse2);

            //!(IsRiding & pressing)
            if (useKey)  //if (!(useKey && playerController.currentRide != null))
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
                            return;

                        if (playerController.CanMove())
                        {
                            //useable.LookingAt();

                            if (useKey)
                                useable.Use();
                        }
                    }
                }
            }
        }
    }

    public interface Useable
    {
        public abstract void Use();
    }
}