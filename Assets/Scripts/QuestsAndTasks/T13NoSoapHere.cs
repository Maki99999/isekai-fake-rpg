using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T13NoSoapHere : MonoBehaviour, Useable
    {
        public Outline outline;
        private OutlineHelper outlineHelper;

        private void Awake()
        {
            outlineHelper = new OutlineHelper(this, outline);
        }

        void Update()
        {
            outlineHelper.UpdateOutline();
        }

        public void LookingAt()
        {
            outlineHelper.ShowOutline();
        }

        public void Use()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "No Soap here. I'd better do it upstairs." });
        }
    }
}
