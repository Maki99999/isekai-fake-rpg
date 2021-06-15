using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class CrowdSfx : MonoBehaviour
    {
        public Transform audioTransform;
        public Bounds crowdArea;

        Transform player;

        void Start()
        {
            player = GameController.Instance.gamePlayer.transform;
        }

        void Update()
        {
            Vector3 playerPositionRotated = RotatePointAroundPivot(player.position, transform.position, Quaternion.Inverse(transform.rotation));
            Vector3 closestPointRotated = transform.position + crowdArea.ClosestPoint(playerPositionRotated - transform.position);
            audioTransform.transform.position = RotatePointAroundPivot(closestPointRotated, transform.position, transform.rotation);
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
        {
            return angles * (point - pivot) + pivot;
        }
    }
}
