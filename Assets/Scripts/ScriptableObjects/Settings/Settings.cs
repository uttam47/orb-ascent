using UnityEngine;
using System.IO;

namespace AnalyticalApproach.OrbAscent
{

    public abstract class Settings: ScriptableObject
    {
        private string FILE_PATH => Path.Combine(Application.persistentDataPath, GetType().Name+ ".json");

        public virtual void Load()
        {
            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                Debug.LogWarning($"Settings file not found at {FILE_PATH}. A new one will be created upon saving.");
            }
        }

        public virtual void Save()
        { 
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(FILE_PATH, json);
        }

    }

}