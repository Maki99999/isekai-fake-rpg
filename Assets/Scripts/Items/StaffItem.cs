using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StaffItem : ItemHoldable
    {
        // Override if needed
        public override MoveData UseItem(MoveData inputData)
        {
            return inputData;
        }

        private void Start()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().items.Add(this);
        }
    }
}
