using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class ItemHoldable : MonoBehaviour
    {
        // Override if needed
        public virtual MoveData UseItem(MoveData inputData)
        {
            return inputData;
        }
    }
}
