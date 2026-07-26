using System.IO;
using UnityEngine;

namespace Project.TowerDefense
{
    public class JsonManager
    {
        private readonly string _baseDirectory = Application.persistentDataPath;

        public void Save(string relativePath, object data)
        {
            string fullPath = Path.Combine(_baseDirectory, relativePath);
            string json = JsonUtility.ToJson(data, false);

            try
            {
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(fullPath, json);
                Debug.Log($"[JsonManager] File saved successfully to: {fullPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] Failed to save file to {fullPath}. Error: {e.Message}");
            }
        }

        public T Load<T>(string relativePath) where T : new()
        {
            string fullPath = Path.Combine(_baseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"[JsonManager] Save file not found at: {fullPath}. Returning new instance.");
                return new T();
            }

            try
            {
                string json = File.ReadAllText(fullPath);
                return JsonUtility.FromJson<T>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] Failed to load file from {fullPath}. Error: {e.Message}");
                return new T();
            }
        }

        public bool Exists(string relativePath)
        {
            string fullPath = Path.Combine(_baseDirectory, relativePath);
            return File.Exists(fullPath);
        }

        public void Delete(string relativePath)
        {
            string fullPath = Path.Combine(_baseDirectory, relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.Log($"[SaveService] File deleted: {fullPath}");
            }
        }

        public void ClearAllSaves(string relativeDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(relativeDirectoryPath) || relativeDirectoryPath == "/" || relativeDirectoryPath == "\\")
            {
                Debug.LogWarning("[JsonManager] Attempted to delete root or empty directory! Operation cancelled for safety.");
                return;
            }

            string targetFolder = Path.Combine(_baseDirectory, relativeDirectoryPath);

            if (Directory.Exists(targetFolder))
            {
                Directory.Delete(targetFolder, true);
                Debug.Log($"[JsonManager] All saves cleared in: {targetFolder}");
            }
            else
            {
                Debug.Log($"[JsonManager] Directory {relativeDirectoryPath} does not exist, nothing to delete.");
            }
        }
    }
}
