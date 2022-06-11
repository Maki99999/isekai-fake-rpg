using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class OutlineHelper
    {
        private MonoBehaviour monoBehaviour;
        private Outline[] outlines;

        private bool lastFrameVisible = false;
        private bool visible = false;

        public OutlineHelper(MonoBehaviour monoBehaviour, Outline[] outlines)
        {
            this.monoBehaviour = monoBehaviour;
            this.outlines = outlines;

            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        public OutlineHelper(MonoBehaviour monoBehaviour, Outline outline)
        {
            this.monoBehaviour = monoBehaviour;
            this.outlines = new Outline[] { outline };

            outline.enabled = false;
        }

        public void DisableOutlineDirectly()
        {
            visible = false;
            foreach (Outline outline in outlines)
                outline.enabled = false;
        }

        public void DestroyOutlines()
        {
            foreach (Outline outline in outlines)
                Object.Destroy(outline);

            outlines = new Outline[0];
        }

        public void DestroyOutline(Outline outline)
        {
            outlines = System.Array.FindAll(outlines, o => o != outline);
            Object.Destroy(outline);
        }

        public void ShowOutline()
        {
            visible = true;
        }

        public void UpdateOutline()
        {
            visible = false;
            monoBehaviour.StartCoroutine(UpdateAtEndOfFrame());
        }

        private IEnumerator UpdateAtEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            if (lastFrameVisible != visible)
                foreach (Outline outline in outlines)
                    outline.enabled = visible;

            lastFrameVisible = visible;
        }
    }
}