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
            "{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}}}";

            //Machines done, no Quests done
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"1\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q1\"},\"dictList\":{\"finishedQuests\":[]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T2\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"453,1283;60,08732;620,7281;0;0,2588191;0;0,9659258;1;1;1\",\"rot\":\"453,1283;61,68732;620,7281;0;0,2588191;0;0,9659258;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-11,62743;-0,8700001;5,014137;0;0,3379172;0;0,9411759;1;1;1\",\"rot\":\"-11,62743;0,6299999;5,014137;0,2791004;0,3227174;-0,1002074;0,898841;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"1\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"0\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-15,215;-0,009;-6,41;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-9,343;-1;-0,232;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-9,343;0;-0,232;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-9,343;-1;-0,232;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";

            //Food done, Speak to king quest done
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"0\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q2\"},\"dictList\":{\"finishedQuests\":[\"Q1\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"T13\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"406,6948;60,08732;537,4046;0;-0,8629552;0;-0,5052806;1;1;1\",\"rot\":\"406,6948;61,68732;537,4046;-0,0044095;-0,8629224;0,007530868;-0,5052614;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-4,669586;-0,8699999;-0,7139304;0;0,9994427;0;0,03338094;1;1;1\",\"rot\":\"-4,669586;0,6300001;-0,7139304;0,01951818;0,81079;-0,5843846;0,02708002;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"1\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;0,9999999;0,9999999;0,9999999\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";

            //washed hands
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"0\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"Q2\"},\"dictList\":{\"finishedQuests\":[\"Q1\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T8\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"406,6948;60,08732;537,4046;0;-0,8629552;0;-0,5052806;1;1;1\",\"rot\":\"406,6948;61,68732;537,4046;-0,0044095;-0,8629224;0,007530868;-0,5052614;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-15,92581;2,13;-3,919238;0;0,806062;0;-0,5918312;1;1;1\",\"rot\":\"-15,92581;3,63;-3,919238;-0,2022495;0,7575341;-0,2754597;-0,5562008;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"1\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"false\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;0,9999999;0,9999999;0,9999999\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";

            //quests q2,3; task getCrackers (before blackout)
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"H4\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"5\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\",\"Q3\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\",\"T8\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"443,3587;60,08732;617,9883;0;-0,1564359;0;0,9876881;1;1;1\",\"rot\":\"443,3587;61,68732;617,9883;0,1884607;-0,1535617;0,02984953;0,9695413;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-21,00009;2,13;0,5865988;0;0,9905133;0;-0,1374174;1;1;1\",\"rot\":\"-21,00009;3,63;0,5865988;-0,03287208;0,9617558;-0,236944;-0,1334278;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"3\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;-0,1074732;0;0,9942082;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;0,9999999;0,9999999;0,9999999\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"false\"},\"dictList\":{}}}";

            //got knife (what comes next: last quest, then find phone)
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"5\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\",\"Q3\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"T14\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\",\"T8\",\"T12\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"443,3587;60,08734;617,9883;0;0,3379152;0;0,9411766;1;1;1\",\"rot\":\"443,3587;61,68734;617,9883;0,1747461;0,3320397;-0,06273993;0,924812;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-15,86256;-0,8700002;-1,886341;0;0,3535014;0;-0,935434;1;1;1\",\"rot\":\"-15,86256;0,6299998;-1,886341;-0,2084075;0,3446165;-0,07875739;-0,9119229;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"2\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"3\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;0,7578842;0;0,6523893;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;0,9999999;0,9999999;0,9999999\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"true\"},\"dictList\":{}}}";

            //everything done, waiting for ending
            //"{\"info\":{\"dictString\":{\"version\":\"0.1\"},\"dictList\":{}},\"horrorEventManager\":{\"dictString\":{\"delayedEvent\":\"\"},\"dictList\":{}},\"musicManager\":{\"dictString\":{\"currentType\":\"5\"},\"dictList\":{}},\"questManager\":{\"dictString\":{\"currentQuestId\":\"\"},\"dictList\":{\"finishedQuests\":[\"Q1\",\"Q2\",\"Q3\",\"Q4\"]}},\"saveManager\":{\"dictString\":{\"currentTaskId\":\"\",\"taskOnDelay\":\"Ending\",\"taskOnDelayWaitQuest\":\"\"},\"dictList\":{\"finishedTasks\":[\"T4\",\"T2\",\"T13\",\"T8\",\"T12\",\"T14\"]}},\"GamePlayer\":{\"dictString\":{\"pos\":\"382,2336;60,38733;541,0828;0;0,8093531;0;0,5873225;1;1;1\",\"rot\":\"382,2336;61,98732;541,0828;0,05812409;0,8053799;-0,08009724;0,5844393;1;1;1\"},\"dictList\":{}},\"MetaPlayer\":{\"dictString\":{\"pos\":\"-9,343868;-0,9308869;3,210194;0;0,999926;0;-0,01216346;1;1;1\",\"rot\":\"-9,343868;0,5691131;3,210194;-0,004779852;0,9194841;-0,392939;-0,01118494;1;1;1\"},\"dictList\":{}},\"H1Call\":{\"dictString\":{\"nextCallClip\":\"0\"},\"dictList\":{}},\"king\":{\"dictString\":{\"currentState\":\"5\"},\"dictList\":{}},\"H3Window\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"SpiderHideBedroom\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideGarage\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"SpiderHideBasement\":{\"dictString\":{\"active\":\"false\"},\"dictList\":{}},\"SpiderHideLiving Room\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"H11Glass\":{\"dictString\":{\"triggered\":\"true\"},\"dictList\":{}},\"H10FleshThing\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-12,56054;0,0639;-6,562059;0;0,7578842;0;0,6523893;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlate\":{\"dictString\":{\"fleshOn\":\"true\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;0,9999999;0,9999999;0,9999999\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"H10PizzaFleshPlateFake\":{\"dictString\":{\"fleshOn\":\"false\",\"transform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\",\"plateFallTransform\":\"-4,636223;-0,8699999;-1,212816;0;0;0;1;1;1;1\"},\"dictList\":{}},\"SpiderHideKitchen\":{\"dictString\":{\"active\":\"true\"},\"dictList\":{}},\"knifeItem\":{\"dictString\":{\"enabled\":\"true\",\"equipped\":\"true\"},\"dictList\":{}}}";

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
                if (saveObject != null && saveDict != null && saveDict.ContainsKey(saveObject.saveDataId))
                    saveObject.Load(saveDict[saveObject.saveDataId]);
                else if (saveObject != null)
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

        public static string GetGameObjectPath(Transform transform)
        {
            Transform lastTransform = transform;
            string path = lastTransform.name;
            while (lastTransform.parent != null)
            {
                lastTransform = lastTransform.parent;
                path = lastTransform.name + "/" + path;
            }
            return path;
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

        public void Add(string key, int value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
        }

        public void Add(string key, float value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
        }

        public void Add(string key, bool value)
        {
            if (dictList.ContainsKey(key) || dictString.ContainsKey(key))
            {
                Debug.LogError("Key already present");
                return;
            }
            dictString.Add(key, value.ToString());
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

        public int GetInt(string key, int defaultValue)
        {
            int returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && int.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
            else
                return defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            float returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && float.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
            else
                return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            bool returnValue = defaultValue;
            bool returnSuccessful = dictString.ContainsKey(key) && bool.TryParse(dictString[key], out returnValue);
            if (returnSuccessful)
                return returnValue;
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
