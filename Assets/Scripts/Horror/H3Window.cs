using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class H3Window : MonoBehaviour, ISaveDataObject
    {
        public GameObject normalWindow;
        public GameObject brokenWindow;

        public AudioSource audio1;
        public AudioSource audio2;

        bool triggered = false;

        public string saveDataId => "H3Window";

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
                return;
            if (dictEntry.GetString("triggered", "false") == "true")
            {
                triggered = true;
                normalWindow.SetActive(false);
                brokenWindow.SetActive(true);
            }
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("triggered", triggered ? "true" : "false");
            return entry;
        }

        public void Trigger()
        {
            if (triggered)
                return;
            triggered = true;

            GameController.Instance.horrorEventManager.StartEvent("H12");

            StartCoroutine(GameController.Instance.playerEventManager.FocusObject(false, transform, 2f));
            normalWindow.SetActive(false);
            brokenWindow.SetActive(true);
            audio1.PlayDelayed(0.25f);
            audio2.PlayDelayed(0.65f);
        }
    }
}
