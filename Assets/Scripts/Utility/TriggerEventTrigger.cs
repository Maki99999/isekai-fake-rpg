using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class TriggerEventTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent unityEvent;
        [SerializeField] private string triggerTag = "Player";
        [SerializeField] private bool disableAfterTriggering;

        private void OnTriggerEnter(Collider other)
        {
            if (enabled && other.CompareTag(triggerTag))
            {
                unityEvent.Invoke();
                if (disableAfterTriggering)
                    enabled = false;
            }
        }
    }
}
