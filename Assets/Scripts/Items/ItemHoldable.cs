using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ItemHoldable : MonoBehaviour
    {
        public PosRotScale positionWhenHeld;
        public string itemName;
        // Override if needed
        public virtual MoveData UseItem(MoveData inputData)
        {
            return inputData;
        }

        public virtual void OnUnequip() { }
        public virtual void OnEquip() { }
    }

    [System.Serializable]
    public class PosRotScale
    {
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 scale = Vector3.one;
    }
}
