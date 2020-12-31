using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class DialogueChoice : MonoBehaviour
    {
        public DialogueManager dialogueManager;
        public int choiceNumber = 0;

        public void ChooseThis()
        {
            dialogueManager.pressedChoice = choiceNumber;
        }
    }
}