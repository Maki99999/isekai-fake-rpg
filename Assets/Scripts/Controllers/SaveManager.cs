using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;

namespace Default
{
    public class SaveManager : MonoBehaviour
    {
        public Animator saveAnim;
        private static readonly string pathSuffix = "/progress.sav";

        private ICollection<ISaveDataObject> saveObjects;

        #region Debug
        public bool debugGame;
        private static readonly string debugGameFile =
            //Start
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}}}";
            //T4
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q1\"},\"dictList\":{\"finishedQuests\":[]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T2\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-15,215;-0,009;-6,41;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate2\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-9,343;0;-0,232;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";
            //T2
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q2\"},\"dictList\":{\"finishedQuests\":[\"Q1\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"T13\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T2\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,776506;-0,8700001;-1,260042;-7,44967E-09;0;5,820765E-11;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";
            //T13
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q2\"},\"dictList\":{\"finishedQuests\":[\"Q1\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T13\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";
            //T8
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q2\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T8\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";
            //T12
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q3\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T14\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T8\",\"T12\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562063;1,129504E-09;0,7806123;-1,044877E-08;-0,6250156;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"true\"},\"dictList\":{}}}";
            //T14
            "{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\",\"Q3\",\"Q4\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T8\",\"T12\",\"T14\"]}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562063;1,201314E-09;0,7380794;-1,111306E-08;-0,6747141;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,776506;-0,8700001;-1,260042;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"true\"},\"dictList\":{}}}";
        #endregion

        public void SaveGameNextFrame()
        {
            StartCoroutine(SaveGame());
        }

        private void Awake()
        {
            saveObjects = GetSaveDataObjects();
        }

        private IEnumerator SaveGame()
        {
            yield return new WaitForEndOfFrame();
            string path = Application.persistentDataPath + pathSuffix;

            SaveDataEntry generalInfo = new SaveDataEntry();
            generalInfo.Add("version", Application.version);

            Dictionary<string, SaveDataEntry> saveDict = new Dictionary<string, SaveDataEntry>();
            saveDict.Add("info", generalInfo);
            foreach (ISaveDataObject saveObject in saveObjects)
                saveDict.Add(saveObject.saveDataId, saveObject.Save());

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(saveDict);

            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(jsonString);
            writer.Close();

            Debug.Log(jsonString);
            Debug.Log("Saved Game to:\n" + path);
            saveAnim.SetTrigger("Saved");
        }

        public void LoadGame()
        {
            string path = Application.persistentDataPath + pathSuffix;
            Dictionary<string, SaveDataEntry> saveDict = null;
            if (debugGame)
                saveDict = JsonConvert.DeserializeObject<Dictionary<string, SaveDataEntry>>(debugGameFile);
            else if (!File.Exists(path))
            {
                Debug.Log("New Save.");
            }
            else
            {
                StreamReader reader = new StreamReader(path);
                string jsonString = reader.ReadLine();
                reader.Close();

                saveDict = JsonConvert.DeserializeObject<Dictionary<string, SaveDataEntry>>(jsonString);

                SaveDataEntry info = saveDict["info"];
                Debug.Log("Loaded save file for version " + info.GetString("version", "null"));
            }

            foreach (ISaveDataObject saveObject in saveObjects)
                if (saveDict != null && saveDict.ContainsKey(saveObject.saveDataId))
                    saveObject.Load(saveDict[saveObject.saveDataId]);
                else
                    saveObject.Load(null);
        }

        public ICollection<ISaveDataObject> GetSaveDataObjects()
        {
            var saveables = new List<ISaveDataObject>();
            var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in rootObjs)
                saveables.AddRange(root.GetComponentsInChildren<ISaveDataObject>(true));
            return saveables;
        }

        public static string TransformToString(Transform transform)
        {
            return $"{transform.position.x};{transform.position.y};{transform.position.z};{transform.rotation.x};{transform.rotation.y};{transform.rotation.z};{transform.rotation.w};{transform.localScale.x};{transform.localScale.y};{transform.localScale.z}";
        }

        public static void ApplyStringToTransform(Transform transform, string transformString)
        {
            string[] transformPartStrings = transformString.Split(';');
            float[] transformParts = new float[transformPartStrings.Length];
            for (int i = 0; i < transformPartStrings.Length; i++)
                transformParts[i] = float.Parse(transformPartStrings[i]);
            transform.position = new Vector3(transformParts[0], transformParts[1], transformParts[2]);
            transform.rotation = new Quaternion(transformParts[3], transformParts[4], transformParts[5], transformParts[6]);
            transform.localScale = new Vector3(transformParts[7], transformParts[8], transformParts[9]);
        }
    }

    public interface ISaveDataObject
    {
        string saveDataId { get; }

        SaveDataEntry Save();
        void Load(SaveDataEntry dictEntry);
    }

    [System.Serializable]
    public class SaveDataEntry
    {
        [JsonProperty] private Dictionary<string, string> dictString { get; set; }
        [JsonProperty] private Dictionary<string, List<string>> dictList { get; set; }

        public SaveDataEntry()
        {
            dictString = new Dictionary<string, string>();
            dictList = new Dictionary<string, List<string>>();
        }

        public void Add(string key, string value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value);
        }

        public void Add(string key, List<string> values)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictList.Add(key, values);
        }

        public string GetString(string key, string defaultValue)
        {
            if (dictString.ContainsKey(key))
                return dictString[key];
            else
                return defaultValue;
        }

        public List<string> GetList(string key, List<string> defaultValue)
        {
            if (dictList.ContainsKey(key))
                return dictList[key];
            else
                return defaultValue;
        }
    }
}
