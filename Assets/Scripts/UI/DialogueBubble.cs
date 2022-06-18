using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace Default
{
    public class DialogueBubble : MonoBehaviour
    {
        public CanvasGroup cGroup;
        public RectTransform mainRect;
        public RectTransform bubbleRect;
        public RectTransform pointerArrowRect;
        public GameObject pointer;
        public Text text;
        public Text invisibleText;

        public Transform followPosition;

        private bool isHidden = true;
        private Camera uiCam;

        void Start()
        {
            uiCam = GameController.Instance.gamePlayer.cam;

            mainRect.localRotation = Quaternion.Euler(Vector3.zero);
            mainRect.localPosition = Vector3.zero;
            mainRect.pivot = new Vector2(0.5f, 0f);
        }

        private void LateUpdate()
        {
            if (!isHidden)
                MoveBubble();
        }

        public IEnumerator WriteText(string textToWrite, float secondsPerCharacter = 0.05f, float secondsAfterText = 5f)
        {
            SetHidden(false);

            invisibleText.text = textToWrite;
            text.text = "";

            foreach (char letter in textToWrite)
            {
                text.text += letter;
                yield return new WaitForSeconds(secondsPerCharacter);
            }

            yield return new WaitForSeconds(secondsAfterText);
            SetHidden(true);
        }

        public IEnumerator WriteTexts(string[] textsToWrite, float secondsPerCharacter = 0.05f, float secondsBetweenTexts = 4f)
        {
            if (textsToWrite == null || textsToWrite.Length <= 0)
                yield return null;
            else
                foreach (string textToWrite in textsToWrite)
                    yield return WriteText(textToWrite, secondsPerCharacter, secondsBetweenTexts);
        }

        public void SetHidden(bool hidden)
        {
            isHidden = hidden;
            if (hidden)
                cGroup.alpha = 0f;
            else
                cGroup.alpha = 1f;
        }

        void MoveBubble()
        {
            Vector3 screenCenter = new Vector3((uiCam.pixelWidth * GameController.uiScaleFactor), (uiCam.pixelHeight * GameController.uiScaleFactor), 0) / 2f;
            Vector3 screenBounds = screenCenter - Vector3.right * (bubbleRect.sizeDelta.x / 2f + 10) - Vector3.up * (bubbleRect.sizeDelta.y / 2f + 10);

            Vector3 screenPosition = uiCam.WorldToScreenPoint(followPosition.position) * GameController.uiScaleFactor;
            bool isTargetVisible = screenPosition.z > 0
                    && screenPosition.x > (bubbleRect.sizeDelta.x / 2f) + 10
                    && screenPosition.x < (uiCam.pixelWidth * GameController.uiScaleFactor) - (bubbleRect.sizeDelta.x / 2f) - 10
                    && screenPosition.y > 0
                    && screenPosition.y < (uiCam.pixelHeight * GameController.uiScaleFactor) - bubbleRect.sizeDelta.y - 20;


            if (!isTargetVisible)
            {
                float angle;
                BubblePositionAndAngleBorder(ref screenPosition, out angle, screenCenter, screenBounds);

                pointer.SetActive(false);
                pointerArrowRect.gameObject.SetActive(true);
                pointerArrowRect.rotation = Quaternion.Euler(0f, 0f, (angle * Mathf.Rad2Deg * -1f) - 90f);
            }
            else
            {
                pointer.SetActive(true);
                pointerArrowRect.gameObject.SetActive(false);
            }

            mainRect.anchoredPosition = screenPosition;
        }

        private void BubblePositionAndAngleBorder(ref Vector3 screenPosition, out float angle, Vector3 screenCenter, Vector3 screenBounds)
        {
            screenPosition -= screenCenter;
            screenPosition += Vector3.up * (bubbleRect.sizeDelta.y / 2f + 10);

            if (screenPosition.z < 0)
                screenPosition *= -1;

            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            float slope = Mathf.Tan(angle);

            if (screenPosition.x > 0)
                screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
            else
                screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);

            if (screenPosition.y > screenBounds.y)
                screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            else if (screenPosition.y < -screenBounds.y)
                screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);

            screenPosition -= Vector3.up * (bubbleRect.sizeDelta.y / 2f + 10);
            screenPosition += screenCenter;
        }
    }
}
