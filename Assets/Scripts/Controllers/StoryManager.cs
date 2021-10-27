using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StoryManager : SaveDataObject
    {
        public SaveManager saveManager;
        public bool startNormally = true;
        public bool startInPcMode = false;
        [Space(5)]
        public Trailer trailer;
        public PcController pcController;

        [Space(10)]
        public T2Oven t2Obj;
        public T4WashingMachine t4Obj;
        public T8Cracker t8Obj;
        public T10Trash t10Obj;
        public T11Dishes t11Obj;
        public T12GetKnife t12Obj;
        public T13WashHands t13Obj;
        public T14MissingPhone t14Obj;

        public H16Puppet puppet;

        public string currentTaskId { get; private set; } = "";
        public override string saveDataId { get => "saveManager"; }

        private HashSet<string> finishedTasks = new HashSet<string>();

        void Start()
        {
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
            if (startNormally)
                saveManager.LoadGame();
            else if (startInPcMode)
                pcController.ToPcModeInstant();
        }

        public override SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("finishedTasks", new List<string>(finishedTasks));

            return new SaveDataEntry();
        }

        public override void Load(SaveDataEntry dictEntry)
        {
            Debug.LogWarning("TODO: Load this!");
            if (dictEntry == null)
                trailer.StartTrailer();
        }

        public bool isTaskBlockingPc()
        {
            switch (currentTaskId)
            {
                case "T2":
                    return t2Obj.BlockingPcMode();
                case "T4":
                    return t4Obj.BlockingPcMode();
                case "T8":
                    return t8Obj.BlockingPcMode();
                case "T12":
                    return t12Obj.BlockingPcMode();
                case "T13":
                    return t13Obj.BlockingPcMode();
                case "T14":
                    return t14Obj.BlockingPcMode();
                default:
                    return false;
            }
        }

        public void StartTask(string id, bool sideTask = false)
        {
            if (!sideTask)
                currentTaskId = id;
            switch (id)
            {
                case "T2":
                    t2Obj.gameObject.SetActive(true);
                    break;
                case "T4":
                    t4Obj.gameObject.SetActive(true);
                    break;
                case "T8":
                    t8Obj.gameObject.SetActive(true);
                    break;
                case "T10":
                    t10Obj.gameObject.SetActive(true);
                    break;
                case "T11":
                    t11Obj.gameObject.SetActive(true);
                    break;
                case "T12":
                    t12Obj.gameObject.SetActive(true);
                    break;
                case "T14":
                    puppet.enabled = true;
                    t14Obj.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void TaskFinished()
        {
            string finishedTask = currentTaskId;

            finishedTasks.Add(finishedTask);
            Debug.Log("Task " + finishedTask + " finished.");
            saveManager.SaveGame();

            switch (currentTaskId)
            {
                case "T4":
                    StartCoroutine(StartNextTaskDelayed("T2", "Q1"));
                    break;
                case "T2":
                    StartCoroutine(StartNextTaskDelayed("T8", "Q2"));
                    break;
                case "T8":
                    StartCoroutine(StartNextHorrorEventDelayed("H4"));
                    break;
                case "T12":
                    StartCoroutine(StartNextTaskDelayed("T14", "Q4"));
                    break;
                case "T14":
                    puppet.enabled = false;
                    //End
                    break;
                default:
                    break;
            }
            StartTask("");
        }

        private IEnumerator StartNextTaskDelayed(string nextTaskId, string questToWaitFor = "")
        {
            if (questToWaitFor != "")
                yield return new WaitUntil(() => GameController.Instance.questManager.IsQuestDone(questToWaitFor));
            yield return new WaitUntil(() => GameController.Instance.inPcMode);
            yield return new WaitForSeconds(27f);
            StartTask(nextTaskId);
        }

        private IEnumerator StartNextHorrorEventDelayed(string nextHorrorEventId)
        {
            yield return new WaitUntil(() => GameController.Instance.inPcMode);
            yield return new WaitForSeconds(27f);
            GameController.Instance.horrorEventManager.StartEvent(nextHorrorEventId);
        }
    }
}
