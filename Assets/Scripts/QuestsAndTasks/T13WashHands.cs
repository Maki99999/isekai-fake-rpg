using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T13WashHands : MonoBehaviour, Useable, Task
    {
        public Outline outline;
        public GameObject noSoapDownstairsObject;
        public Sink sink;

        private bool inUse = false;

        private void Start()
        {
            sink.enabled = false;
            noSoapDownstairsObject.SetActive(true);
        }

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
            if (!inUse)
            {
                StartCoroutine(WashHandsAnim());
            }
        }

        public bool BlockingPcMode()
        {
            GameController.Instance.dialogue.StartDialogueWithFreeze(new List<string>() { "I want to wash hands first." });
            return true;
        }

        IEnumerator WashHandsAnim()
        {
            yield return sink.WashHandsAnim(true);

            noSoapDownstairsObject.SetActive(false);
            GameController.Instance.storyManager.TaskFinished();
            sink.enabled = true;
        }

        public void SkipTask() { }
    }
}
