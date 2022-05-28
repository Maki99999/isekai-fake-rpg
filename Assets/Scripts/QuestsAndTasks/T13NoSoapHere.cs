using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T13NoSoapHere : MonoBehaviour, Useable
    {
        public Outline outline;

        void Update()
        {
            outline.enabled = false;
        }

        public void LookingAt()
        {
            outline.enabled = true;
        }

        public void Use()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "No Soap here. I'd better do it upstairs." });
        }
    }
}
