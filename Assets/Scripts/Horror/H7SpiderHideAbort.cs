using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H7SpiderHideAbort : MonoBehaviour
    {
        public H7SpiderHide h7;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                h7.Hide2();
        }
    }
}
