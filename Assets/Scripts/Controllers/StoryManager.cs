using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Default
{
    public class StoryManager : MonoBehaviour, ISaveDataObject
    {
        public Trailer trailer;
        public Ending ending;

        [Space(10)]
        public T2Oven t2Obj;
        public T4WashingMachine t4Obj;
        public T8Cracker t8Obj;
        public T12GetKnife t12Obj;
        public T13WashHands t13Obj;
        public T14MissingPhone t14Obj;

        public Dictionary<string, Task> tasks;

        public string currentTaskId { get; private set; } = "";
        public string saveDataId { get => "saveManager"; }

        private HashSet<string> finishedTasks = new HashSet<string>() { };
        private string taskOnDelay = "";
        private string taskOnDelayWaitQuest = "";

        private void Awake()
        {
            tasks = new Dictionary<string, Task>()
            {
                {"T2", t2Obj},
                {"T4", t4Obj},
                {"T8", t8Obj},
                {"T12", t12Obj},
                {"T13", t13Obj},
                {"T14", t14Obj}
            };
        }

        private IEnumerator Start()
        {
            yield return null;
            yield return null;
            GameController.Instance.playerEventManager.FreezePlayer(false, false);
        }

        public SaveDataEntry Save()
        {
            SaveDataEntry entry = new SaveDataEntry();
            entry.Add("finishedTasks", new List<string>(finishedTasks));
            entry.Add("currentTaskId", currentTaskId);
            entry.Add("taskOnDelay", taskOnDelay);
            entry.Add("taskOnDelayWaitQuest", taskOnDelayWaitQuest);
            return entry;
        }

        public void Load(SaveDataEntry dictEntry)
        {
            if (dictEntry == null)
            {
                trailer.StartTrailer();
                return;
            }
            foreach (string taskId in dictEntry.GetList("finishedTasks", new List<string>()))
                tasks[taskId].SkipTask();

            StartTask(dictEntry.GetString("currentTaskId", ""));

            string taskOnDelay = dictEntry.GetString("taskOnDelay", "");
            string taskOnDelayWaitQuest = dictEntry.GetString("taskOnDelayWaitQuest", "");
            if (taskOnDelay != "")
                StartCoroutine(StartNextTaskDelayed(taskOnDelay, taskOnDelayWaitQuest));
        }

        public bool isTaskBlockingPc()
        {
            if (currentTaskId == "")
                return false;
            return tasks[currentTaskId].BlockingPcMode();
        }

        public void StartTask(string id)
        {
            if (id == "Ending")
                ending.StartEnding();
            else if (id != "")
            {
                if (tasks.ContainsKey(id))
                    ((MonoBehaviour)tasks[id]).gameObject.SetActive(true);
            }
            currentTaskId = id;
        }

        public void TaskFinished(string taskId = null)
        {
            if (taskId != null && currentTaskId != taskId)
            {
                Debug.LogWarning("Task to end was not the current task.");
                return;
            }
            string finishedTask = currentTaskId;

            finishedTasks.Add(finishedTask);
            Debug.Log("Task " + finishedTask + " finished.");
            GameController.Instance.saveManager.SaveGameNextFrame();

            switch (currentTaskId)
            {
                case "T4":
                    StartCoroutine(StartNextTaskDelayed("T2", "Q1"));
                    break;
                case "T2":
                    StartCoroutine(StartNextTaskDelayed("T8", "Q3"));
                    break;
                case "T8":
                    GameController.Instance.horrorEventManager.StartEventDelayed("H4");
                    break;
                case "T12":
                    StartCoroutine(StartNextTaskDelayed("T14", "Q4"));
                    break;
                case "T14":
                    StartCoroutine(StartNextTaskDelayed("Ending"));
                    break;
                default:
                    break;
            }
            StartTask("");
        }

        private IEnumerator StartNextTaskDelayed(string nextTaskId, string questToWaitFor = "")
        {
            if (taskOnDelay != "")
                Debug.LogError("Cannot schedule two tasks at a time!");

            taskOnDelay = nextTaskId;
            taskOnDelayWaitQuest = questToWaitFor;
            if (!System.String.IsNullOrEmpty(questToWaitFor))
                yield return new WaitUntil(() => GameController.Instance.questManager.IsQuestDone(questToWaitFor));
            yield return new WaitUntil(() => GameController.Instance.inPcMode);
            yield return new WaitForSeconds(25f);
            yield return new WaitUntil(() => GameController.Instance.inPcMode);
            taskOnDelay = "";
            taskOnDelayWaitQuest = "";
            StartTask(nextTaskId);
        }
    }

    public interface Task
    {
        bool BlockingPcMode();
        void SkipTask();
    }
}
