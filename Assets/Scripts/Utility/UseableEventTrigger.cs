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

        private OutlineHelper outlineHelper;

        private void Awake()
        {
            outlineHelper = new OutlineHelper(this, outlines);
        }

        private void Update()
        {
            outlineHelper.UpdateOutline();
        }

        private void OnDisable()
        {
            outlineHelper.DisableOutlineDirectly();
        }

        public void LookingAt()
        {
            if (enabled)
                outlineHelper.ShowOutline();
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
