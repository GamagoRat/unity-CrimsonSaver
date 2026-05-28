using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using JetBrains.Annotations;

namespace PhylisiumStudio.CrimsonSaver.Extensions
{
    /// <summary>
    /// Utility class for saving and loading data to and from the persistent data path and PlayerPrefs in Unity.
    /// Async helpers have been added for file IO and JSON (JsonUtility) wrappers.
    /// Note: JsonUtility is CPU-bound and Unity API (PlayerPrefs) must be used on the main thread; async wrappers avoid blocking the caller but do not move Unity API calls to background threads.
    /// </summary>
    public static class FileExtension
    {
        /// <summary>
        /// Saves the given content to a file in the persistent data path with the specified file name.
        /// If the file already exists, it will be overwritten.
        /// </summary>
        /// <param name="fileName">The name of the file to save the content to. This should include the file extension (e.g., "saveData.json").</param>
        /// <param name="content">The content to be saved to the file. This is typically a string representation of the data, such as a JSON string.</param>
        public static void SaveToPersistentDataPath(this string content, string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// Async version of <see cref="SaveToPersistentDataPath"/> : Saves the given content to a file in the persistent data path.
        /// Uses File.WriteAllTextAsync. Exceptions propagate to the caller.
        /// </summary>
        public static async Awaitable SaveToPersistentDataPathAsync(this string content, string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);
            await File.WriteAllTextAsync(path, content);
        }

        /// <summary>
        /// Creates a folder with the specified name in the persistent data path if it does not already exist.
        /// </summary>
        /// <param name="folderName">The name of the folder to create in the persistent data path. This should be a valid folder name without any path separators (e.g., "SaveData").</param>
        public static void CreateFolderInPersistentDataPath(this string folderName)
        {
            var path = Path.Combine(Application.persistentDataPath, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Deletes the folder with the specified name from the persistent data path if it exists. This will also delete all files and subfolders within the folder.
        /// </summary>
        /// <param name="folderName">The name of the folder to delete from the persistent data path. This should be a valid folder name without any path separators (e.g., "SaveData").</param>
        public static void DeleteFolderInPersistentDataPath(this string folderName)
        {
            var path = Path.Combine(Application.persistentDataPath, folderName);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Loads the content from a file in the persistent data path with the specified file name.
        /// If the file does not exist, it returns null.
        /// </summary>
        /// <param name="fileName">The name of the file to load the content from. This should include the file extension (e.g., "saveData.json").</param>
        /// <returns>The content of the file as a string if the file exists; otherwise, null.</returns>
        [CanBeNull]
        public static string LoadFromPersistentDataPath(this string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        /// <summary>
        /// Async version of <see cref="LoadFromPersistentDataPath"/> : Loads the content from a file in the persistent data path.
        /// </summary>
        public static async Awaitable<string> LoadFromPersistentDataPathAsync(this string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(path))
            {
                return null;
            }

            return await File.ReadAllTextAsync(path);
        }

        /// <summary>
        /// Verifies if a file with the specified name exists in the persistent data path.
        /// </summary>
        /// <param name="fileName">The name of the file to check for existence. This should include the file extension (e.g., "saveData.json").</param>
        /// <returns>True if the file exists in the persistent data path; otherwise, false.</returns>
        public static bool ExistsInPersistentDataPath(this string fileName)
        {
            var path = Path.Combine(Application.persistentDataPath, fileName);
            return File.Exists(path);
        }

        /// <summary>
        /// Async wrapper for <see cref="ExistsInPersistentDataPath"/>. The existence check is fast so this just returns a completed task.
        /// </summary>
        public static async Awaitable<bool> ExistsInPersistentDataPathAsync(this string fileName)
        {
            return await fileName.ExistsInPersistentDataPathAsync();
        }

        /// <summary>
        /// Saves the given content to PlayerPrefs with the specified key.
        /// If the key already exists, it will be overwritten.
        /// Note: PlayerPrefs is a Unity API and must be called on the main thread. This method performs the call synchronously but returns a Task for convenience.
        /// </summary>
        /// <param name="key">The key under which the content will be saved in PlayerPrefs. This should be a unique identifier for the data being saved.</param>
        /// <param name="content">The content to be saved in PlayerPrefs. This is typically a string representation of the data, such as a JSON string.</param>
        public static void SaveToPlayerPrefs(this string content, string key)
        {
            PlayerPrefs.SetString(key, content);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Async wrapper for <see cref="SaveToPlayerPrefs"/>. Does not offload to a background thread because Unity APIs must run on the main thread.
        /// </summary>
        public static async Awaitable SaveToPlayerPrefsAsync(this string content, string key)
        {
            content.SaveToPlayerPrefs(key);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Loads the content from PlayerPrefs with the specified key.
        /// If the key does not exist, it returns null.
        /// </summary>
        /// <param name="key">The key under which the content is saved in PlayerPrefs. This should be the same unique identifier used when saving the data.</param>
        /// <returns>The content associated with the specified key in PlayerPrefs as a string if the key exists; otherwise, null.</returns>
        [CanBeNull]
        public static string LoadFromPlayerPrefs(this string key)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : null;
        }

        /// <summary>
        /// Async wrapper for <see cref="LoadFromPlayerPrefs"/>. Returns a completed Task containing the value.
        /// </summary>
        public static async Awaitable<string> LoadFromPlayerPrefsAsync(this string key)
        {
            return await Task.FromResult(key.LoadFromPlayerPrefs());
        }

        /// <summary>
        /// Verifies if a key exists in PlayerPrefs.
        /// </summary>
        /// <param name="key">The key to check for existence in PlayerPrefs. This should be the same unique identifier used when saving the data.</param>
        /// <returns>True if the key exists in PlayerPrefs; otherwise, false.</returns>
        public static bool ExistsInPlayerPrefs(this string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// Async wrapper for <see cref="ExistsInPlayerPrefs"/>. Fast operation, returns completed task.
        /// </summary>
        public static async Awaitable<bool> ExistsInPlayerPrefsAsync(this string key)
        {
            return await Task.FromResult(key.ExistsInPlayerPrefs());
        }
    }
}