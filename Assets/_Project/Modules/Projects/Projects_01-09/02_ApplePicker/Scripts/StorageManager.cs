using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Projects.ApplePicker
{
    [System.Serializable]
    public class DataEntry
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<DataEntry> entries = new List<DataEntry>();
    }

    public class StorageManager
    {

        private string _path = Path.Combine(Application.persistentDataPath, "save.json");

        public void SetValue<T>(string key, T value)
        {
            SaveData data = LoadAll();
            string stringValue = value.ToString();

            var entry = data.entries.Find(e => e.key == key);
            if (entry == null)
            {
                data.entries.Add(new DataEntry { key = key, value = stringValue });
            }
            else
            {
                entry.value = stringValue;
            }

            File.WriteAllText(_path, JsonUtility.ToJson(data, false));
        }

        public T GetValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;

            SaveData data = LoadAll();

            var entry = data.entries.FirstOrDefault(e => e.key == key);

            if (entry == null)
                return default;

            if (entry.value is T typedValue)
                return typedValue;

            try
            {
                return (T)Convert.ChangeType(entry.value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        public SaveData Load()
        {
            if (!File.Exists(_path)) return new SaveData();
            string json = File.ReadAllText(_path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        private SaveData LoadAll()
        {
            if (!File.Exists(_path)) return new SaveData();
            return JsonUtility.FromJson<SaveData>(File.ReadAllText(_path));   
        }
    }
}
