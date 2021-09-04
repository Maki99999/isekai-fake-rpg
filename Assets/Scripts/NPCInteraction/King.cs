using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class King : MonoBehaviour
    {
        private static readonly string[] textQuest1 = {
            "Welcome, hero!",
            "I have summoned\nyou here so you\ncan help us in defeating\nthe evil demon lord!",
            "But first i want\nto see what you\nare capable of.",
            "So your first quest\nis to defeat\n10 slimes.",
            "Come back after\nyou did so." };

        private bool inSpeech = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!inSpeech && other.CompareTag("Player"))
            {
                inSpeech = true;
                StartCoroutine(Dialogue());
            }
        }

        IEnumerator Dialogue()
        {
            yield return GameController.Instance.dialogueBubble.WriteTexts(textQuest1);
            inSpeech = false;
        }
    }
}
