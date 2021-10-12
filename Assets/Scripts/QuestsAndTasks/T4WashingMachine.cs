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

        private int state = 0;  //0:pre washingMachine, 1:T10, 2:post T10/washingMachine, 3:
        private List<string> dialogueStart = new List<string>() { "I should do laundry first.", "The laundry basket is in the bathroom, the washing machine is downstairs below this room." };
        private List<string> dialogueDryer = new List<string>() { "Now the dryer." };
        private List<string> dialogueFinished = new List<string>() { "Okay, laundry's done for today. Let's enjoy the game." };
        private List<string> dialoguePC = new List<string>() { "Laundry is not done yet." };
        private List<string> dialogueT10 = new List<string>() { "I still have trash in the kitchen to bring outside." };
        private List<string> dialogueT11 = new List<string>() { "The dishes in the kitchen are not done yet." };

        private void Start()
        {
            basketUseableCollider.enabled = true;
            GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueStart);

            washingMachine.gameObject.SetActive(true);
            GameController.Instance.horrorEventManager.StartEvent("H5");
        }

        public void MachineStarted()
        {
            state++;
            if (state == 1)
                GameController.Instance.storyManager.StartTask("T10", true);
            else
                GameController.Instance.storyManager.StartTask("T11", true);
        }

        public void MachineShouldFinish()
        {
            state++;
            StartCoroutine(MachineTimer());
        }

        public void MachineEmptied()
        {
            state++;
            if (state == 3)
            {
                dryer.gameObject.SetActive(true);
                washingMachine.gameObject.SetActive(false);
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueDryer);
            }
            else
            {
                dryer.gameObject.SetActive(false);
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueFinished);
                GameController.Instance.storyManager.TaskFinished();
            }
        }

        public bool BlockingPcMode()
        {
            if (state == 1)
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueT10);
            else if (state == 4)
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialogueT11);
            else
                GameController.Instance.dialogue.StartDialogueWithFreeze(dialoguePC);
            return true;
        }

        IEnumerator MachineTimer()
        {
            yield return new WaitForSeconds(machineTime);
            if (state == 2)
                washingMachine.Finish();
            else
                dryer.Finish();
        }
    }
}
