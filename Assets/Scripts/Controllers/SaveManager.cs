using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Default
{
    public class SaveManager : MonoBehaviour
    {
        public Animator saveAnim;
        private static readonly string pathSuffix = "/progress.sav";

        public void SaveGame()
        {
            string path = Application.persistentDataPath + pathSuffix;
            Debug.Log("Saved to: " + path);

            SaveDataEntry generalInfo = new SaveDataEntry();
            generalInfo.Add("version", Application.version);

            SaveDataObject[] saveObjects = GameObject.FindObjectsOfType<SaveDataObject>();
            Dictionary<string, SaveDataEntry> saveDict = new Dictionary<string, SaveDataEntry>();
            saveDict.Add("info", generalInfo);
            foreach (SaveDataObject saveObject in saveObjects)
                saveDict.Add(saveObject.saveDataId, saveObject.Save());

            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(saveDict);

            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(jsonString);
            writer.Close();

            Debug.Log("Saved Game");
            saveAnim.SetTrigger("Saved");
        }

        public void LoadGame()
        {
            string path = Application.persistentDataPath + pathSuffix;
            Dictionary<string, SaveDataEntry> saveDict = null;

            if (!File.Exists(path))
            {
                Debug.Log("No save file found.");
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

            SaveDataObject[] saveObjects = GameObject.FindObjectsOfType<SaveDataObject>();
            foreach (SaveDataObject saveObject in saveObjects)
                if (saveDict != null && saveDict.ContainsKey(saveObject.saveDataId))
                    saveObject.Load(saveDict[saveObject.saveDataId]);
                else
                    saveObject.Load(null);
        }
    }

    public abstract class SaveDataObject : MonoBehaviour
    {
        public abstract string saveDataId { get; }

        public abstract SaveDataEntry Save();
        public abstract void Load(SaveDataEntry dictEntry);
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
