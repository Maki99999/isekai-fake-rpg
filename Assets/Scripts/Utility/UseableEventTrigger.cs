using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Default
{
    public class UseableEventTrigger : MonoBehaviour, Useable
    {
        [SerializeField] private UnityEvent unityEvent;
        [SerializeField] private Outline[] outlines;
        [SerializeField] private bool disableAfterTriggering;

        private void Update()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        private void OnDisable()
        {
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        public void LookingAt()
        {
            if (enabled)
                foreach (Outline outline in outlines)
                    outline.enabled = true;
        }

        public void Use()
        {
            if (!enabled)
                return;

            unityEvent.Invoke();
            if (disableAfterTriggering)
                enabled = false;
        }
    }
}
