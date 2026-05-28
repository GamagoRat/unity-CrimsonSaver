using PhylisiumStudio.CrimsonSaver.Extensions;
using UnityEngine;

namespace PhylisiumStudio.CrimsonSaver.Saver
{
    /// <summary>
    /// Persistent Data Path Saver implementation of the <see cref="ISaver<T>"/>. interface for saving and loading data to and from
    /// the PersistentDataPath using a key-value approach.
    /// </summary>
    public class PersistentDataPathSaver : ISaver
    {
        private string folderPath;

        public PersistentDataPathSaver(string folderPath = "")
        {
            this.folderPath = folderPath;
            folderPath.CreateFolderInPersistentDataPath();
        }

        public void Save(string key, string data)
        {
            data.SaveToPersistentDataPath(KeyFormat(key));
        }

        public string Load(string key)
        {
            return KeyFormat(key).LoadFromPersistentDataPath();
        }

        public bool Exists(string key)
        {
            return KeyFormat(key).ExistsInPersistentDataPath();
        }

        public Awaitable SaveAsync(string key, string data)
        {
            return data.SaveToPersistentDataPathAsync(KeyFormat(key));
        }

        public Awaitable<string> LoadAsync(string key)
        {
            return KeyFormat(key).LoadFromPersistentDataPathAsync();
        }

        public Awaitable<bool> ExistsAsync(string key)
        {
            return KeyFormat(key).ExistsInPersistentDataPathAsync();
        }

        public string KeyFormat(string key)
        {
            return $"{folderPath}/{key}.json";
        }
    }
}