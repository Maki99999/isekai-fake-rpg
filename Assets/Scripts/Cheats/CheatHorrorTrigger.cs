using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Default
{
    public class CheatHorrorTrigger : MonoBehaviour
    {
        public float resetTime = 3f;
        float lastPress = -1f;
        string currentCode = "";

        public Text text;

        void Update()
        {
            CheckNumpad();
        }

        void CheckNumpad()
        {
            string codeAddition = CurrentlyPressedButtons();
            if (Time.time > lastPress + resetTime)
            {
                currentCode = "";
                if (text != null)
                    text.text = currentCode;
            }
            if (!NumpadActive())
                return;

            lastPress = Time.time;

            currentCode += codeAddition;
            if (currentCode.Length > 3)
                currentCode = currentCode.Substring(1, 3);
            if (text != null)
                text.text = currentCode;
            if (currentCode.Length == 3)
            {
                int codeNum = int.Parse(currentCode);
                Debug.Log("CheatHorrorTrigger: Event H" + codeNum);
                GameController.Instance.horrorEventManager.StartEvent("H" + codeNum);
                if (text != null)
                    text.text = "H" + codeNum;
                currentCode = "";
            }
        }

        bool NumpadActive()
        {
            return (Input.GetKeyDown(KeyCode.Keypad0)) ||
                   (Input.GetKeyDown(KeyCode.Keypad1)) ||
                   (Input.GetKeyDown(KeyCode.Keypad2)) ||
                   (Input.GetKeyDown(KeyCode.Keypad3)) ||
                   (Input.GetKeyDown(KeyCode.Keypad4)) ||
                   (Input.GetKeyDown(KeyCode.Keypad5)) ||
                   (Input.GetKeyDown(KeyCode.Keypad6)) ||
                   (Input.GetKeyDown(KeyCode.Keypad7)) ||
                   (Input.GetKeyDown(KeyCode.Keypad8)) ||
                   (Input.GetKeyDown(KeyCode.Keypad9));
        }

        string CurrentlyPressedButtons()
        {
            string pressedButtons = "";
            if (Input.GetKeyDown(KeyCode.Keypad0))
                pressedButtons += "0";
            if (Input.GetKeyDown(KeyCode.Keypad1))
                pressedButtons += "1";
            if (Input.GetKeyDown(KeyCode.Keypad2))
                pressedButtons += "2";
            if (Input.GetKeyDown(KeyCode.Keypad3))
                pressedButtons += "3";
            if (Input.GetKeyDown(KeyCode.Keypad4))
                pressedButtons += "4";
            if (Input.GetKeyDown(KeyCode.Keypad5))
                pressedButtons += "5";
            if (Input.GetKeyDown(KeyCode.Keypad6))
                pressedButtons += "6";
            if (Input.GetKeyDown(KeyCode.Keypad7))
                pressedButtons += "7";
            if (Input.GetKeyDown(KeyCode.Keypad8))
                pressedButtons += "8";
            if (Input.GetKeyDown(KeyCode.Keypad9))
                pressedButtons += "9";
            return pressedButtons;
        }
    }
}
