using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class T4WashingMachine : MonoBehaviour
    {
        public Collider basketUseableCollider;
        public WashingDryerMachine washingMachine;
        public WashingDryerMachine dryer;
        public float machineTime = 1f;

        private bool dryingMode = false;
        private List<string> dialogueStart = new List<string>() { "I should do laundry first.", "The laundry basket is in the bathroom, the washing machine is downstairs below this room." };
        private List<string> dialogueDryer = new List<string>() { "Now the dryer." };
        private List<string> dialogueFinished = new List<string>() { "Okay, laundry's done for today." };
        private List<string> dialoguePC = new List<string>() { "Laundry is not done yet." };

        private void Start()
        {
            basketUseableCollider.enabled = true;
            GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueStart);

            washingMachine.gameObject.SetActive(true);
        }

        public void MachineStarted()
        {
            StartCoroutine(MachineTimer());
        }

        public void MachineEmptied()
        {
            if (!dryingMode)
            {
                dryingMode = true;
                dryer.gameObject.SetActive(true);
                washingMachine.gameObject.SetActive(false);
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueDryer);
            }
            else
            {
                dryer.gameObject.SetActive(false);
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueFinished);
                GameController.Instance.storyManager.TaskFinished(); //TODO: cooperation with T10
            }
        }

        public bool BlockingPcMode()
        {
            //TODO: cooperation with T10
            GameController.Instance.dialogue.StartDialogueWithFreeze(dialoguePC);
            return true;
        }

        IEnumerator MachineTimer()
        {
            yield return new WaitForSeconds(machineTime);
            if (dryingMode)
                dryer.Finish();
            else
                washingMachine.Finish();
        }
    }
}
